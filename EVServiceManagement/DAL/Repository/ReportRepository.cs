using DAL.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly EVServiceManagementContext _ctx;
        public ReportRepository(EVServiceManagementContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<int> GetTodayAppointmentsAsync()
        {
            var today = DateTime.Today;
            return await _ctx.Appointments.CountAsync(a => a.Date.Date == today);
        }

        public async Task<int> GetLowStockPartsAsync()
        {
            return await _ctx.Parts.CountAsync(p => p.StockQuantity < 5);
        }

        public async Task<int> GetActiveCustomersAsync()
        {
            return await _ctx.Customers.CountAsync();
        }

        public async Task<decimal> GetMonthlyRevenueAsync()
        {
            var now = DateTime.Now;
            return await _ctx.Payments
                .Where(p => p.Status == "Paid"
                    && p.CreatedAt.HasValue
                    && p.CreatedAt.Value.Month == now.Month
                    && p.CreatedAt.Value.Year == now.Year)
                .SumAsync(p => (decimal?)p.TotalAmount ?? 0);
        }

        public async Task<List<(string Month, decimal Revenue)>> GetYearlyRevenueAsync(int year)
        {
            return await _ctx.Payments
                .Where(p => p.CreatedAt.HasValue && p.CreatedAt.Value.Year == year && p.Status == "Paid")
                .GroupBy(p => p.CreatedAt.Value.Month)
                .Select(g => new ValueTuple<string, decimal>(
                    new DateTime(year, g.Key, 1).ToString("MMM"),
                    g.Sum(p => p.TotalAmount)
                ))
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetAppointmentStatusStatsAsync()
        {
            return await _ctx.Appointments
                .GroupBy(a => a.Status)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Key, x => x.Count);
        }
    }
}
