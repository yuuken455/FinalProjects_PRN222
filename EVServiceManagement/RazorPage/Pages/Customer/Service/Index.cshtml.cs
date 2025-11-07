using BLL.IService;
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

        public void OnGet()
        {
        }
    }
}
