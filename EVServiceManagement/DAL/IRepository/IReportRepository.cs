using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepository
{
    public interface IReportRepository
    {
        Task<int> GetTodayAppointmentsAsync();
        Task<int> GetLowStockPartsAsync();
        Task<int> GetActiveCustomersAsync();
        Task<decimal> GetMonthlyRevenueAsync();
        Task<List<(string Month, decimal Revenue)>> GetYearlyRevenueAsync(int year);
        Task<Dictionary<string, int>> GetAppointmentStatusStatsAsync();
    }
}
