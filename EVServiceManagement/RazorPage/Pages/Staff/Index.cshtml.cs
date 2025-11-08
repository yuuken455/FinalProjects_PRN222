using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Staff
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (!int.TryParse(HttpContext.Session.GetString("StaffId"), out int staffId))
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }
    }
}
