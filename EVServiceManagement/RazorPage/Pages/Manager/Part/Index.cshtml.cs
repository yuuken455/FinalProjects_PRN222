using BLL.DTOs.PartDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Manager.Part
{
    public class IndexModel : PageModel
    {
        private readonly IPartService partService;

        public IndexModel(IPartService partService)
        {
            this.partService = partService;
        }

        public ICollection<PartDto> PartDtos { get; set; } = new List<PartDto>();

        public void OnGet()
        {
        }
    }
}
