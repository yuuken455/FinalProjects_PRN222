using BLL.DTOs.Vehicle;

namespace BLL.IService
{
    public interface IVehicleService
    {
        Task<ICollection<VehicleDto>> GetVehiclesByCustomerIdAsync(int customerId);
        Task DeleteVehicleAsync(int vehicleId);
        Task AddVehicleAsync(CreateVehicleDto createVehicleDto);
        Task UpdateVehicleAsync(UpdateVehicleDto updateVehicleDto);
        Task<VehicleDto?> GetVehicleByIdAsync(int vehicleId);
    }
}
