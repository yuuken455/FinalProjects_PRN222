using BLL.DTOs.PartDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Manager.Part
{
    public class EditModel : PageModel
    {
        private readonly IPartService _service;
        public EditModel(IPartService service) { _service = service; }

        [BindProperty] public UpdatePartDto Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var p = await _service.GetAsync(id);
            if (p == null) return NotFound();
            Input = new UpdatePartDto
            {
                PartId = p.PartId,
                Name = p.Name,
                UnitPrice = p.UnitPrice,
                StockQuantity = p.StockQuantity,
                Status = p.Status
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            await _service.UpdateAsync(Input);
            TempData["Msg"] = "Đã cập nhật linh kiện.";
            return RedirectToPage("Index");
        }
    }
}
