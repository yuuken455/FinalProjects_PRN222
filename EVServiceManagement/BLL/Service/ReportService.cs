using BLL.DTOs.ReportDtos;
using BLL.IService;
using DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;
        public ReportService(IReportRepository repo)
        {
            _repo = repo;
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            var now = DateTime.Now;
            var year = now.Year;

            var dto = new DashboardDto
            {
                TodayAppointments = await _repo.GetTodayAppointmentsAsync(),
                LowStockParts = await _repo.GetLowStockPartsAsync(),
                ActiveCustomers = await _repo.GetActiveCustomersAsync(),
                MonthlyRevenue = await _repo.GetMonthlyRevenueAsync()
            };

            var yearly = await _repo.GetYearlyRevenueAsync(year);
            dto.Months = yearly.Select(x => x.Month).ToList();
            dto.Revenues = yearly.Select(x => x.Revenue).ToList();
            dto.AppointmentStatus = await _repo.GetAppointmentStatusStatsAsync();

            return dto;
        }
    }
}
