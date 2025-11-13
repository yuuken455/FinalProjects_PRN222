using BLL.DTOs.AccountDtos;
using BLL.DTOs.AppointmentDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Staff.Appointment
{
    public class DetailModel : PageModel
    {
        private readonly IAppointmentService appointmentService;
        private readonly ITechnicianService technicianService;

        public DetailModel(IAppointmentService appointmentService, ITechnicianService technicianService)
        {
            this.appointmentService = appointmentService;
            this.technicianService = technicianService;
        }

        public AppointmentDto AppointmentDto { get; set; } = new AppointmentDto();
        public ICollection<TechnicianDto> FreeTechnicians { get; set; } = new List<TechnicianDto>();

        [BindProperty]
        public int? MainTechnicianId { get; set; }
        [BindProperty]
        public int? AssistantTechnicianId { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("StaffId")))
            {
                return RedirectToPage("/Login");
            }

            AppointmentDto = await appointmentService.GetAppointmentByIdAsync(id) ?? new AppointmentDto();

            var duration = AppointmentDto.ServiceOrderDetailDtos?.Where(x => x.ServiceDto != null).Sum(x => x.ServiceDto!.Duration) ?? 0;
            FreeTechnicians = await technicianService.GetTechniciansCanWork(AppointmentDto.Date, duration);

            return Page();
        }

        public async Task<IActionResult> OnPostAssign(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("StaffId")))
            {
                return RedirectToPage("/Login");
            }

            var dto = await appointmentService.GetAppointmentByIdAsync(id);
            if (dto == null) return NotFound();
            if (!string.Equals(dto.Status, "Pending", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Err"] = "Can't assign technician.";
                return RedirectToPage(new { id });
            }

            dto.TechnicianAssignmentDtos.Clear();
            if (MainTechnicianId.HasValue)
            {
                dto.TechnicianAssignmentDtos.Add(new TechnicianAssignmentDto
                {
                    AppointmentId = id,
                    TechnicianId = MainTechnicianId.Value,
                    AssignedAt = DateTime.Now,
                    Role = "Main"
                });
            }
            if (AssistantTechnicianId.HasValue && AssistantTechnicianId != MainTechnicianId)
            {
                dto.TechnicianAssignmentDtos.Add(new TechnicianAssignmentDto
                {
                    AppointmentId = id,
                    TechnicianId = AssistantTechnicianId.Value,
                    AssignedAt = DateTime.Now,
                    Role = "Assistant"
                });
            }

            if (MainTechnicianId.HasValue)
            {
                dto.Status = "Scheduled";
            }

            await appointmentService.UpdateAppointment(dto);
            TempData["Msg"] = "Assign technician successfully.";
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostCancel(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("StaffId")))
            {
                return RedirectToPage("/Login");
            }

            var dto = await appointmentService.GetAppointmentByIdAsync(id);
            if (dto == null) return NotFound();
            if (!string.Equals(dto.Status, "Pending", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Err"] = "Can't cancel.";
                return RedirectToPage(new { id });
            }

            dto.Status = "Canceled";
            await appointmentService.UpdateAppointment(dto);
            TempData["Msg"] = "Cancelled Appointment!";
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostPay(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("StaffId")))
            {
                return RedirectToPage("/Login");
            }

            await appointmentService.MarkPaymentPaidAsync(id);
            TempData["Msg"] = "Bill paid!";
            return RedirectToPage(new { id });
        }
    }
}
