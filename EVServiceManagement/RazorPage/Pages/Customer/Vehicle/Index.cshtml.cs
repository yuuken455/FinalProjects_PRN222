using BLL.DTOs.Vehicle;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Customer.Vehicle
{
    public class IndexModel : PageModel
    {
        private readonly IVehicleService vehicleService;

        public IndexModel(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        public ICollection<VehicleDto> MyVehicle { get; set; } = new List<VehicleDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!int.TryParse(HttpContext.Session.GetString("CustomerId"), out int customerId))
            {
                return RedirectToPage("/Login");
            }

            var vehicles = await vehicleService.GetVehiclesByCustomerIdAsync(customerId);
            MyVehicle = vehicles;

            return Page();
        }
    }
}
