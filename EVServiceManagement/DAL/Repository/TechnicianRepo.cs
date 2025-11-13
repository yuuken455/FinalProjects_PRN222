using DAL.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class TechnicianRepo : ITechnicianRepo
    {
        private readonly EVServiceManagementContext dbContext;

        public TechnicianRepo(EVServiceManagementContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<Technician>> GetTechniciansCanWork(DateTime startTime, int duration)
        {
            var endTime = startTime.AddMinutes(duration);

            var free = await dbContext.Technicians
                .Where(t => !t.TechnicianAssignments.Any(
                    ta => ta.Appointment.Status == "Scheduled" &&
                          ta.Appointment.Date < endTime &&
                          ta.Appointment.Date.AddMinutes(duration) > startTime))
                .Include(t => t.Account)    
                .ToListAsync() ;
            return free;
        }
    }
}
