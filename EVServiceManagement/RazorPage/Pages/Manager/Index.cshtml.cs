using BLL.DTOs.PartDtos;
using BLL.DTOs.ReportDtos;
using BLL.IService;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Manager
{
    public class IndexModel : PageModel
    {
        private readonly IReportService _reportService;
        public IndexModel(IReportService reportService) { _reportService = reportService; }

        public DashboardDto Data { get; set; } = new();

        public async Task OnGetAsync()
        {
            Data = await _reportService.GetDashboardAsync();
        }
    }
}
