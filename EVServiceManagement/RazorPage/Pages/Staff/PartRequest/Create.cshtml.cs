using BLL.DTOs.PartDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Staff.PartRequest
{
    public class CreateModel : PageModel
    {
        private readonly IPartRequestService _service;
        private readonly IPartService _partService;

        public CreateModel(IPartRequestService s, IPartService p)
        {
            _service = s;
            _partService = p;
        }

        public List<PartDto> Parts { get; set; } = new();
        [BindProperty] public CreatePartRequestDto Input { get; set; } = new();

        // ✅ Nhận partId từ route
        public async Task OnGetAsync(int partId)
        {
            Parts = await _partService.GetAllAsync();
            Input.RequestedBy = 1; // TODO: lấy từ Claims/session

            // ✅ Gán sẵn linh kiện nếu có
            if (partId > 0)
            {
                Input.PartId = partId;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Parts = await _partService.GetAllAsync();
            if (!ModelState.IsValid) return Page();

            Input.RequestedBy = 1; // TODO: lấy từ Claims/session
            await _service.CreateAsync(Input);
            TempData["Msg"] = "✅ Đã gửi yêu cầu linh kiện thành công!";
            return RedirectToPage("MyRequests");
        }
    }
}
