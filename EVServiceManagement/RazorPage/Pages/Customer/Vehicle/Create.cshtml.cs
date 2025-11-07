using BLL.DTOs.Vehicle;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Customer.Vehicle
{
    public class CreateModel : PageModel
    {
        private readonly IVehicleService vehicleService;

        public CreateModel(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        [BindProperty]
        public CreateVehicleDto CreateVehicleDto { get; set; } = new CreateVehicleDto();
        public string ErrorMessage { get; set; } = string.Empty;    

        public IActionResult OnGet()
        {
            if (!int.TryParse(HttpContext.Session.GetString("CustomerId"), out int customerId))
            {
                return RedirectToPage("/Login");    
            }

            CreateVehicleDto.CustomerId = customerId;   

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await vehicleService.AddVehicleAsync(CreateVehicleDto);
            } catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
            return RedirectToPage("/Customer/Vehicle/Index");
        }   
    }
}
