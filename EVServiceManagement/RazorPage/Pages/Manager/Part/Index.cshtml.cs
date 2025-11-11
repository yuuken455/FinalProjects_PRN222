using BLL.DTOs.PartDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Manager.Part
{
    public class IndexModel : PageModel
    {
        private readonly IPartService _service;
        public IndexModel(IPartService service) { _service = service; }

        public List<PartDto> Parts { get; set; } = new();
        [BindProperty(SupportsGet = true)] public string? Keyword { get; set; }

        public async Task OnGetAsync()
        {
            Parts = await _service.GetAllAsync(Keyword);
        }
    }
}
