using DAL.Entities;

namespace DAL.IRepository
{
    public interface IVehicleRepo
    {
        Task<ICollection<Vehicle>> GetVehiclesByCustomerIdAsync(int customerId);
        Task DeleteVehicleAsync(Vehicle vehicle);
        Task UpdateVehicleAsync(Vehicle vehicle);
        Task<Vehicle?> GetVehicleByIdAsync(int vehicleId);  
        Task AddVehicleAsync(Vehicle vehicle);
    }
}
