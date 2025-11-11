using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.ReportDtos
{
    public class DashboardDto
    {
        public int TodayAppointments { get; set; }
        public int LowStockParts { get; set; }
        public int ActiveCustomers { get; set; }
        public decimal MonthlyRevenue { get; set; }

        public List<string> Months { get; set; } = new();
        public List<decimal> Revenues { get; set; } = new();
        public Dictionary<string, int> AppointmentStatus { get; set; } = new();
    }
}
