using BLL.DTOs.AppointmentDtos;
using BLL.DTOs.ServiceDtos;
using BLL.DTOs.VehicleDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Customer.Appointment
{
    public class CreateModel : PageModel
    {
        private readonly IAppointmentService appointmentService;
        private readonly IVehicleService vehicleService;
        private readonly IServiceService serviceService;

        public CreateModel(IAppointmentService appointmentService, IVehicleService vehicleService, IServiceService serviceService)
        {
            this.appointmentService = appointmentService;
            this.vehicleService = vehicleService;
            this.serviceService = serviceService;
        }

        public ICollection<VehicleDto> MyVehicle { get; set; } = new List<VehicleDto>();
        public ICollection<ServiceDto> Services { get; set; } = new List<ServiceDto>();
        [BindProperty]
        public CreateAppointmentDto CreateAppointmentDto { get; set; } = new CreateAppointmentDto();
        public string ErrorMessage { get; set; } = string.Empty;    

        public async Task<IActionResult> OnGetAsync()
        {
            if (!int.TryParse(HttpContext.Session.GetString("CustomerId"), out int customerId))
            {
                return RedirectToPage("/Login");
            }
            MyVehicle = await vehicleService.GetVehiclesByCustomerIdAsync(customerId);
            Services = await serviceService.GetAllServices();

            // Default datetime to now (local time)
            if (CreateAppointmentDto.Date == default)
            {
                var now = DateTime.Now;
                // round to next 5 minutes for nicer UX
                var minutes = (int)Math.Ceiling(now.Minute / 5.0) * 5;
                if (minutes == 60) { now = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddHours(1); }
                else { now = new DateTime(now.Year, now.Month, now.Day, now.Hour, minutes, 0); }
                CreateAppointmentDto.Date = now;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                CreateAppointmentDto.CustomerId = int.Parse(HttpContext.Session.GetString("CustomerId") ?? "0");
                await appointmentService.CreateAppointment(CreateAppointmentDto);
                return RedirectToPage("/Customer/Appointment/Index");
            } catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                await OnGetAsync(); 
                return Page();
            }
        }
    }
}
