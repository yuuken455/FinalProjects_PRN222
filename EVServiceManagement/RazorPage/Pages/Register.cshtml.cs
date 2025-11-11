using BLL.DTOs.AccountDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IAccountService accountService;

        public RegisterModel(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [BindProperty]
        public CreateCustomerDto CreateCustomerDto { get; set; } = new();
        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await accountService.AddCustomerAsync(CreateCustomerDto);
                return RedirectToPage("/Login");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
