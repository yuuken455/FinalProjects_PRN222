using BLL.DTOs.PartDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Manager.PartRequest
{
    public class IndexModel : PageModel
    {
        private readonly IPartRequestService _service;
        public IndexModel(IPartRequestService s) { _service = s; }
        public List<PartRequestDto> Items { get; set; } = new();

        public async Task OnGetAsync()
        {
            Items = await _service.ListAsync(status: "Pending");
        }
    }
}
