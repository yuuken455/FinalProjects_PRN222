using DAL.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class VehicleRepo : IVehicleRepo
    {
        private readonly EVServiceManagementContext dbContext;

        public VehicleRepo(EVServiceManagementContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            await dbContext.Vehicles.AddAsync(vehicle);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteVehicleAsync(Vehicle vehicle)
        {
            dbContext.Vehicles.Remove(vehicle);
            await dbContext.SaveChangesAsync();
        }

        public Task<Vehicle?> GetVehicleByIdAsync(int vehicleId)
        {
            return dbContext.Vehicles
                .Include(v => v.Appointments)
                .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);
        }

        public async Task<ICollection<Vehicle>> GetVehiclesByCustomerIdAsync(int customerId)
        {
            return await dbContext.Vehicles
                .Where(v => v.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            dbContext.Vehicles.Update(vehicle); 
            await dbContext.SaveChangesAsync(); 
        }
    }
}
