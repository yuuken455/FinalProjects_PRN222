using DAL.Entities;

namespace DAL.IRepository
{
    public interface IServiceRepo
    {
        Task<ICollection<Service>> GetAllServicesAsync();
    }
}
