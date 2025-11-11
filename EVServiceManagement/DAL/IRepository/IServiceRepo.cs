using DAL.Entities;

namespace DAL.IRepository
{
    public interface IServiceRepo
    {
        Task<ICollection<Service>> GetAllServicesAsync();
        Task<Service?> GetServiceByIdAsync(int serviceId);  
    }
}
