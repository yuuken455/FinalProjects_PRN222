using BLL.DTOs.AppointmentDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Customer.Appointment
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService appointmentService;

        public IndexModel(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        public ICollection<AppointmentDto> AppointmentDtos { get; set; } = new List<AppointmentDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!int.TryParse(HttpContext.Session.GetString("CustomerId"), out int customerId))
            {
                return RedirectToPage("/Login");
            }

            AppointmentDtos = await appointmentService.GetAppointmentsByCustomerIdAsync(customerId);
            return Page();
        }

        public async Task<IActionResult> OnPostCancelAsync(int id, string note)
        {
            if (!int.TryParse(HttpContext.Session.GetString("CustomerId"), out int customerId))
            {
                return RedirectToPage("/Login");
            }

            var ap = await appointmentService.GetAppointmentByIdAsync(id);

            if (ap == null || ap.CustomerId != customerId)
            {
                return RedirectToPage("/Error");
            }

            if (string.Equals(ap.Status, "Pending", StringComparison.OrdinalIgnoreCase))
            {
                await appointmentService.DeletePendingAppointmentAsync(id);
                TempData["Msg"] = "Đã xoá lịch hẹn.";
            }
            else if (string.Equals(ap.Status, "Scheduled", StringComparison.OrdinalIgnoreCase))
            {
                await appointmentService.CancelAppointmentAsync(id, note);
                TempData["Msg"] = "Đã huỷ lịch hẹn.";
            }

            AppointmentDtos = await appointmentService.GetAppointmentsByCustomerIdAsync(customerId);
            return Page();
        }
    }
}
