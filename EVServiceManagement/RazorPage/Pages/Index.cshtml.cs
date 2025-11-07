using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("AccountId") != null)
            {
                switch (HttpContext.Session.GetString("Role"))
                {
                    case "Customer":
                        return RedirectToPage("/Customer/Index");
                    case "Staff":
                        return RedirectToPage("/Staff/Index");
                    case "Technician":
                        return RedirectToPage("/Technician/Index");
                    case "Manager":
                        return RedirectToPage("/Manager/Index");
                    default:
                        HttpContext.Session.Clear();
                        return RedirectToPage("/Login");
                }
            }
            else
            {
                return RedirectToPage("/Login");
            }
        }
    }
}
