using BLL.DTOs.ServiceDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Customer.Service
{
    public class IndexModel : PageModel
    {
        private readonly IServiceService serviceService;

        public IndexModel(IServiceService serviceService)
        {
            this.serviceService = serviceService;
        }

        public ICollection<ServiceDto> ServiceDtos { get; set; } = new List<ServiceDto>();  

        public async Task<IActionResult> OnGetAsync()
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("CustomerId")))
            {
                return RedirectToPage("/Login");
            }

            ServiceDtos = await serviceService.GetAllServices();
            return Page();
        }
    }
}
