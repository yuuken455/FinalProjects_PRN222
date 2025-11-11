using BLL.DTOs.PartDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Staff.Part
{
    public class IndexModel : PageModel
    {
        private readonly IPartService _partService;
        public IndexModel(IPartService partService) { _partService = partService; }

        public ICollection<PartDto> PartDtos { get; set; } = new List<PartDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            // Nếu bạn quản lý session đăng nhập:
            // if (!int.TryParse(HttpContext.Session.GetString("StaffId"), out _)) return RedirectToPage("/Login");

            PartDtos = await _partService.GetAllAsync(); // ✅ dùng GetAllAsync thay cho GetAllParts
            return Page();
        }
    }
}
