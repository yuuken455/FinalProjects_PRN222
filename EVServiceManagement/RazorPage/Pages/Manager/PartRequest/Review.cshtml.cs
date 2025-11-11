using BLL.DTOs.PartDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Manager.PartRequest
{
    public class ReviewModel : PageModel
    {
        private readonly IPartRequestService _service;
        public ReviewModel(IPartRequestService s) { _service = s; }

        public PartRequestDto? Item { get; set; }
        [BindProperty] public string? Notes { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Item = await _service.GetAsync(id);
            return Item == null ? NotFound() : Page();
        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            var managerId = /* TODO: lấy từ Claims */ 1;
            await _service.ApproveAsync(new ApprovePartRequestDto { RequestId = id, ManagerId = managerId, Approve = true, Notes = Notes });
            TempData["ReqMsg"] = $"Đã duyệt yêu cầu #{id}.";
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            var managerId = /* TODO: lấy từ Claims */ 1;
            await _service.ApproveAsync(new ApprovePartRequestDto { RequestId = id, ManagerId = managerId, Approve = false, Notes = Notes });
            TempData["ReqMsg"] = $"Đã từ chối yêu cầu #{id}.";
            return RedirectToPage("Index");
        }
    }
}
