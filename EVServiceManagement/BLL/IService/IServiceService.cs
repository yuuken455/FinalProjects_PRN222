using BLL.DTOs.ServiceDtos;

namespace BLL.IService
{
    public interface IServiceService
    {
        Task<ICollection<ServiceDto>> GetAllServices();
    }
}
