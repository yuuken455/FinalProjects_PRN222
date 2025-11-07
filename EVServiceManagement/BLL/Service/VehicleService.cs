using AutoMapper;
using BLL.DTOs.Vehicle;
using BLL.IService;
using DAL.IRepository;

namespace BLL.Service
{
    public class VehicleService : IVehicleService
    {
        private readonly IMapper mapper;
        private readonly IVehicleRepo vehicleRepo;
        private readonly ICustomerRepo customerRepo;

        public VehicleService(IMapper mapper, IVehicleRepo vehicleRepo, ICustomerRepo customerRepo)
        {
            this.mapper = mapper;
            this.vehicleRepo = vehicleRepo;
            this.customerRepo = customerRepo;
        }

        public async Task AddVehicleAsync(CreateVehicleDto createVehicleDto)
        {
            var customer = await customerRepo.GetCustomerByIdAsync(createVehicleDto.CustomerId);
            if (customer == null)
            {
                throw new Exception("Customer not found");
            }
            if (string.IsNullOrEmpty(createVehicleDto.LicensePlate))
            {
                throw new Exception("License plate is required");
            }
            if (string.IsNullOrEmpty(createVehicleDto.Model))
            {
                throw new Exception("Model is required");
            }
            if (createVehicleDto.CurrentKm < 0)
            {
                throw new Exception("Current KM cannot be negative");
            }
            if (!string.IsNullOrEmpty(createVehicleDto.Vin) && createVehicleDto.Vin.Length != 17)
            {
                throw new Exception("VIN must be 17 characters long");
            }
            
            var vehicle = mapper.Map<DAL.Entities.Vehicle>(createVehicleDto);
            vehicle.Status = "Active";
            await vehicleRepo.AddVehicleAsync(vehicle);
        }

        public async Task DeleteVehicleAsync(int vehicleId)
        {
            var vehicle = await vehicleRepo.GetVehicleByIdAsync(vehicleId);
            if (vehicle == null)
            {
                throw new Exception("Vehicle not found");
            } 
            if (vehicle.Appointments == null || !vehicle.Appointments.Any())
            {
                await vehicleRepo.DeleteVehicleAsync(vehicle);
            } else
            {
                vehicle.Status = "Inactive";    
                await vehicleRepo.UpdateVehicleAsync(vehicle);
            }
        }

        public async Task<VehicleDto?> GetVehicleByIdAsync(int vehicleId)
        {
            var vehicle =  await vehicleRepo.GetVehicleByIdAsync(vehicleId);
            if (vehicle == null)
            {
                return null;
            }
            return mapper.Map<VehicleDto>(vehicle);
        }

        public async Task<ICollection<VehicleDto>> GetVehiclesByCustomerIdAsync(int customerId)
        {
            var vehicles = await vehicleRepo.GetVehiclesByCustomerIdAsync(customerId);

            return mapper.Map<ICollection<VehicleDto>>(vehicles);
        }

        public async Task UpdateVehicleAsync(UpdateVehicleDto updateVehicleDto)
        {
            var vehicle = await vehicleRepo.GetVehicleByIdAsync(updateVehicleDto.VehicleId);
            if (vehicle == null)
            {
                throw new Exception("Vehicle not found");
            }
            if (string.IsNullOrEmpty(updateVehicleDto.LicensePlate))
            {
                throw new Exception("License plate is required");
            }
            if (string.IsNullOrEmpty(updateVehicleDto.Model))
            {
                throw new Exception("Model is required");
            }
            if (updateVehicleDto.CurrentKm < 0)
            {
                throw new Exception("Current KM cannot be negative");
            }
            if (!string.IsNullOrEmpty(updateVehicleDto.Vin) && updateVehicleDto.Vin.Length != 17)
            {
                throw new Exception("VIN must be 17 characters long");
            }
            mapper.Map(updateVehicleDto, vehicle);
            await vehicleRepo.UpdateVehicleAsync(vehicle);
        }
    }
}
