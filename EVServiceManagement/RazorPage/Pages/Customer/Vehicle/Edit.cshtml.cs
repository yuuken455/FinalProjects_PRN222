using BLL.DTOs.VehicleDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Customer.Vehicle
{
    public class EditModel : PageModel
    {
        private readonly IVehicleService vehicleService;

        public EditModel(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        [BindProperty]
        public UpdateVehicleDto UpdateVehicleDto { get; set; } = new UpdateVehicleDto();
        public VehicleDto VehicleDto { get; set; } = new VehicleDto();
        public string ErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CustomerId")))
            {
                return RedirectToPage("/Login");
            }

            if (id == null)
            {
                return RedirectToPage("/Customer/Vehicle/Index");
            }

            var vehicle = await vehicleService.GetVehicleByIdAsync(id.Value);
            if (vehicle == null)
            {
                return RedirectToPage("/Customer/Vehicle/Index");
            }

            VehicleDto = vehicle;

            // Prefill the Update DTO so inputs show current values
            UpdateVehicleDto.VehicleId = vehicle.VehicleId;
            UpdateVehicleDto.Model = vehicle.Model;
            UpdateVehicleDto.Vin = vehicle.Vin;
            UpdateVehicleDto.LicensePlate = vehicle.LicensePlate;
            UpdateVehicleDto.CurrentKm = vehicle.CurrentKm;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await vehicleService.UpdateVehicleAsync(UpdateVehicleDto);
                return RedirectToPage("/Customer/Vehicle/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
