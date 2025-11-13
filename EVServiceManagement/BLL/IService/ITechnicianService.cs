using BLL.DTOs.AccountDtos;

namespace BLL.IService
{
    public interface ITechnicianService
    {
        Task<ICollection<TechnicianDto>> GetTechniciansCanWork(DateTime startTime, int duration);
    }
}
