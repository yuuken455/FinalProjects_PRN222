using BLL.DTOs.PartDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Staff.PartRequest
{
    public class MyRequestsModel : PageModel
    {
        private readonly IPartRequestService _service;

        public MyRequestsModel(IPartRequestService s) { _service = s; }

        public List<PartRequestDto> Items { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy StaffId từ session (hoặc Claims nếu bạn đã cấu hình Identity)
            if (!int.TryParse(HttpContext.Session.GetString("StaffId"), out var staffId))
            {
                // Chưa đăng nhập
                return RedirectToPage("/Login");
            }

            Items = await _service.ListAsync(staffId: staffId);
            return Page();
        }

        public async Task<IActionResult> OnPostReceiveAsync(int id)
        {
            if (!int.TryParse(HttpContext.Session.GetString("StaffId"), out var staffId))
            {
                return RedirectToPage("/Login");
            }

            try
            {
                await _service.ReceiveAsync(new ReceivePartRequestDto
                {
                    RequestId = id,
                    StaffId = staffId,
                    Received = true
                });

                TempData["Msg"] = $"Đã xác nhận nhận đủ cho yêu cầu #{id}.";
            }
            catch (Exception ex)
            {
                TempData["Err"] = ex.Message;
            }

            return RedirectToPage();
        }
    }
}
