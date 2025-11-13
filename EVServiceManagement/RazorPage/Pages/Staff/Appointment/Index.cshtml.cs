using BLL.DTOs.AppointmentDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace RazorPage.Pages.Staff.Appointment
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService appointmentService;

        public IndexModel(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        public ICollection<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();

        public async Task<IActionResult> OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("StaffId")))
            {
                return RedirectToPage("/Login");
            }

            Appointments = await appointmentService.GetAppointments();  

            return Page();
        }
    }
}
