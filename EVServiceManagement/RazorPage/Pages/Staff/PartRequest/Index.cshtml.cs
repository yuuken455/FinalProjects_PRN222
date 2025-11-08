using BLL.DTOs.PartDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Staff.PartRequest
{
    public class IndexModel : PageModel
    {
        private readonly IPartRequestService partRequestService;

        public IndexModel(IPartRequestService partRequestService)
        {
            this.partRequestService = partRequestService;
        }

        public ICollection<PartRequestDto> PartRequestDtos { get; set; } = new List<PartRequestDto>();

        public async Task<IActionResult> OnGet()
        {
            if (!int.TryParse(HttpContext.Session.GetString("StaffId"), out int staffId))
            {
                return RedirectToPage("/Login");
            }

            PartRequestDtos = await partRequestService.GetPartRequestByStaffId(staffId);
            return Page();
        }

        public async Task<IActionResult> OnPostComplete(int id)
        {
            if (!int.TryParse(HttpContext.Session.GetString("StaffId"), out int staffId))
            {
                return RedirectToPage("/Login");
            }

            // TODO: G?i service c?p nh?t tr?ng thái sang "Completed" khi có API h? tr?
            TempData["StatusMessage"] = $"?ã g?i yêu c?u hoàn t?t cho PartRequest #{id}.";

            // Reload data to reflect any changes (n?u service ?ã c?p nh?t)
            PartRequestDtos = await partRequestService.GetPartRequestByStaffId(staffId);
            return RedirectToPage();
        }
    }
}
