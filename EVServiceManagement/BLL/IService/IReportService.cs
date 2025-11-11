using BLL.DTOs.ReportDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IService
{
    public interface IReportService
    {
        Task<DashboardDto> GetDashboardAsync();
    }
}
