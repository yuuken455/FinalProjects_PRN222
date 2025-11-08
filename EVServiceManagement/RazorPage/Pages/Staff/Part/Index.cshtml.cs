using BLL.DTOs.PartDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Staff.Part
{
    public class IndexModel : PageModel
    {
        private readonly IPartService partService;

        public IndexModel(IPartService partService)
        {
            this.partService = partService;
        }

        public ICollection<PartDto> PartDtos { get; set; } = new List<PartDto>();   

        public async Task<IActionResult> OnGetAsync()
        {
            if (!int.TryParse(HttpContext.Session.GetString("StaffId"), out int staffId))
            {
                return RedirectToPage("/Login") ;
            }

            PartDtos = await partService.GetAllParts();
            return Page();
        }
    }
}
