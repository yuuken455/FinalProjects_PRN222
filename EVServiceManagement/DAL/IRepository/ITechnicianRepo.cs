using DAL.Entities;

namespace DAL.IRepository
{
    public interface ITechnicianRepo
    {
        Task<ICollection<Technician>> GetTechniciansCanWork(DateTime startTime, int duration);
    }
}
