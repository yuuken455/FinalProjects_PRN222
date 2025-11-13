using BLL.DTOs.AppointmentDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Technician
{
    public class IndexModel : PageModel
    {
        private readonly ITechnicianService technicianService;
        private readonly IAppointmentService appointmentService;

        public IndexModel(ITechnicianService technicianService, IAppointmentService appointmentService)
        {
            this.technicianService = technicianService;
            this.appointmentService = appointmentService;
        }

        public ICollection<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();

        public async Task<IActionResult> OnGet()
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("TechnicianId")))
            {
                return RedirectToPage("/Login");  
            }

            Appointments = await appointmentService.GetAppointmentsByTechnician(int.Parse(HttpContext.Session.GetString("TechnicianId")!)); 
            return Page();
        }
    }
}
