using BLL.DTOs.PartDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Manager.Part
{
    public class DeleteModel : PageModel
    {
        private readonly IPartService _service;
        public DeleteModel(IPartService service) { _service = service; }

        public PartDto? Part { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Part = await _service.GetAsync(id);
            return Part == null ? NotFound() : Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _service.DeleteAsync(id);
            TempData["Msg"] = "Đã xoá linh kiện.";
            return RedirectToPage("Index");
        }
    }
}
