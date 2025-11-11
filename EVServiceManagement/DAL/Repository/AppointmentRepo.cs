using DAL.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class AppointmentRepo : IAppointmentRepo
    {
        private readonly EVServiceManagementContext dbContext;

        public AppointmentRepo(EVServiceManagementContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAppointment(Appointment appointment)
        {
            await dbContext.Appointments.AddAsync(appointment);
            await dbContext.SaveChangesAsync(); 
        }

        public async Task<ICollection<Appointment>> GetAppointmentsByCustomerId(int customerId)
        {
            return await dbContext.Appointments.
                Where(appointment => appointment.CustomerId == customerId)
                .Include(appointment => appointment.Vehicle)
                .Include(appointment => appointment.Payment)
                .Include(appointment => appointment.ServiceOrderDetails)
                .Include(appointment => appointment.TechnicianAssignments)
                .ThenInclude(ta => ta.Technician)
                .ThenInclude(t => t.Account)    
                .ToListAsync();
        }
    }
}
