using DAL.Entities;

namespace DAL.IRepository
{
    public interface IAppointmentRepo
    {
        Task<ICollection<Appointment>> GetAppointmentsByCustomerId(int customerId);
    }
}
