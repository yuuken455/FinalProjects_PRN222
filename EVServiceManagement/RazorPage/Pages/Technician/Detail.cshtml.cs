using BLL.DTOs.AppointmentDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Technician
{
    public class DetailModel : PageModel
    {
        private readonly IAppointmentService appointmentService;

        public DetailModel(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        public AppointmentDto AppointmentDto { get; set; } = new AppointmentDto();

        public async Task<IActionResult> OnGet(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("TechnicianId")))
            {
                return RedirectToPage("/Login");
            }

            AppointmentDto = await appointmentService.GetAppointmentByIdAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPostComplete(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("TechnicianId")))
            {
                return RedirectToPage("/Login");
            }

            var note = Request.Form["note"].ToString();
            var dict = new Dictionary<int, int>();
            foreach (var key in Request.Form.Keys)
            {
                if (key.StartsWith("actual[", StringComparison.Ordinal))
                {
                    var closing = key.LastIndexOf(']');
                    var idPart = closing > 7 ? key.Substring(7, closing - 7) : string.Empty; // between actual[ and ]
                    if (int.TryParse(idPart, out var detailId))
                    {
                        var values = Request.Form[key];
                        var raw = values.Count > 0 ? values[values.Count - 1] : values.ToString();
                        if (int.TryParse(raw, out var qty))
                        {
                            if (qty < 0) qty = 0;
                            dict[detailId] = qty;
                        }
                    }
                }
            }

            await appointmentService.CompleteAppointmentAsync(id, dict, note);
            TempData["Msg"] = "?ã hoàn t?t l?ch h?n.";
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostCancel(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("TechnicianId")))
            {
                return RedirectToPage("/Login");
            }

            var note = Request.Form["note"].ToString();
            await appointmentService.CancelAppointmentAsync(id, note);
            TempData["Msg"] = "?ã h?y l?ch h?n.";
            return RedirectToPage(new { id });
        }
    }
}
