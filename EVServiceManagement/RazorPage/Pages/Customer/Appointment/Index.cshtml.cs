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
    }
}
