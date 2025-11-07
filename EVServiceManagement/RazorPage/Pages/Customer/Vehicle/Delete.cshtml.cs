using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Customer.Vehicle
{
    public class DeleteModel : PageModel
    {
        private readonly IVehicleService vehicleService;

        public DeleteModel(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        public string ErrorMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                ErrorMessage = "Vehicle ID is required.";
                return Page();
            }

            try
            {
                await vehicleService.DeleteVehicleAsync(id.Value);
                return RedirectToPage("/Customer/Vehicle/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting vehicle: {ex.Message}";
                return Page();
            }
        }
    }
}
