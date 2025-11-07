using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService accountService;

        public LoginModel(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;
        [BindProperty]
        public string Password { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var accountDto = await accountService.LoginAsync(Email, Password);
            if (accountDto != null)
            {
                HttpContext.Session.SetString("AccountId", accountDto.AccountId.ToString());
                if (accountDto.CustomerDto != null)
                {
                    HttpContext.Session.SetString("Role", "Customer");
                    HttpContext.Session.SetString("CustomerId", accountDto.CustomerDto.CustomerId.ToString());
                }
                else if (accountDto.ManagerDto != null)
                {
                    HttpContext.Session.SetString("Role", "Manager");
                    HttpContext.Session.SetString("ManagerId", accountDto.ManagerDto.ManagerId.ToString());
                }
                else if (accountDto.StaffDto != null)
                {
                    HttpContext.Session.SetString("Role", "Staff");
                    HttpContext.Session.SetString("StaffId", accountDto.StaffDto.StaffId.ToString());
                }
                else if (accountDto.TechnicianDto != null)
                {
                    HttpContext.Session.SetString("Role", "Technician");
                    HttpContext.Session.SetString("TechnicianId", accountDto.TechnicianDto.TechnicianId.ToString());
                }
                return RedirectToPage("/Index");
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }
        }
    }
}
