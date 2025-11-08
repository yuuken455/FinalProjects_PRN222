using BLL.DTOs.AppointmentDtos;

namespace BLL.IService
{
    public interface IAppointmentService
    {
        Task<ICollection<AppointmentDto>> GetAppointmentsByCustomerIdAsync(int customerId); 
    }
}
