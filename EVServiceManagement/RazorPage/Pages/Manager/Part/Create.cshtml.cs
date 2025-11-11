using BLL.DTOs.PartDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Manager.Part
{
    public class CreateModel : PageModel
    {
        private readonly IPartService _service;
        public CreateModel(IPartService service) { _service = service; }

        [BindProperty] public CreatePartDto Input { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            await _service.CreateAsync(Input);
            TempData["Msg"] = "Đã thêm linh kiện.";
            return RedirectToPage("Index");
        }
    }
}
