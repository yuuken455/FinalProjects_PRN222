using AutoMapper;
using BLL.DTOs.AppointmentDtos;
using BLL.IService;
using DAL.IRepository;
using Microsoft.AspNetCore.SignalR;
using BLL.Hubs;

namespace BLL.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IMapper mapper;
        private readonly IAppointmentRepo appointmentRepo;
        private readonly ICustomerRepo customerRepo;
        private readonly IServiceRepo serviceRepo;
        private readonly IHubContext<NotificationsHub> hub;

        public AppointmentService(IMapper mapper, IAppointmentRepo appointmentRepo, ICustomerRepo customerRepo, IServiceRepo serviceRepo, IHubContext<NotificationsHub> hub)
        {
            this.mapper = mapper;
            this.appointmentRepo = appointmentRepo;
            this.customerRepo = customerRepo;
            this.serviceRepo = serviceRepo;
            this.hub = hub;
        }

        public async Task CreateAppointment(CreateAppointmentDto createAppointmentDto)
        {
            var checkCustomer = await customerRepo.GetCustomerByIdAsync(createAppointmentDto.CustomerId);
            if (checkCustomer == null)
            {
                throw new Exception("Customer does not exist!");
            }
            if (createAppointmentDto.VehicleId == 0)
            {
                throw new Exception("Select vehicle before book appointment!");
            }
            var checkVehicle = checkCustomer.Vehicles.FirstOrDefault(v => v.VehicleId == createAppointmentDto.VehicleId);
            if (checkVehicle == null)
            {
                throw new Exception("Vehicle does not belong to the customer!");
            }
            if (!createAppointmentDto.CreateServiceOrderDetailDtos.Any())
            {
                throw new Exception("Select service before book appointment!");
            }
            if (createAppointmentDto.Date < DateTime.Now)
            {
                throw new Exception("Appointment date must be in the future!");
            }
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;

            int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)currentDay + 7) % 7;

            if (daysUntilSunday == 0)
                daysUntilSunday = 7;
            DateTime nextSunday = DateTime.Now.AddDays(daysUntilSunday);
            if (createAppointmentDto.Date > nextSunday)
            {
                throw new Exception("Appointment date must be within this week or next week!");
            }

            TimeSpan startOfWork = new(7, 30, 0);
            TimeSpan endOfWork = new(18, 0, 0);

            var duration = 0;
            foreach (var serviceOrderDetailDto in createAppointmentDto.CreateServiceOrderDetailDtos)
            {
                var service = await serviceRepo.GetServiceByIdAsync(serviceOrderDetailDto.ServiceId.Value);
                serviceOrderDetailDto.UnitPrice = service.Price;

                if (string.Equals(service.Type, "Replace", StringComparison.OrdinalIgnoreCase) && service.ServiceParts != null && service.ServiceParts.Any())
                {
                    var partsTotalUnit = service.ServiceParts.Sum(sp => sp.Part.UnitPrice);
                    var qty = serviceOrderDetailDto.Quantity <= 0 ? 1 : serviceOrderDetailDto.Quantity;
                    serviceOrderDetailDto.TotalPrice = service.Price + (qty * partsTotalUnit);
                }
                else
                {
                    serviceOrderDetailDto.TotalPrice = service.Price;
                }

                duration += service?.Duration ?? 0;
            }
            var endDate = createAppointmentDto.Date.AddMinutes(duration);
            if (createAppointmentDto.Date.TimeOfDay < startOfWork || createAppointmentDto.Date.TimeOfDay > endOfWork)
            {
                throw new Exception("Shop open from 7:30AM to 6:00PM!");
            }
            if (endDate.Date > createAppointmentDto.Date.Date || endDate.TimeOfDay > endOfWork)
            {
                throw new Exception($"Your appointment take {duration} minutes to finish but shop close at 6:00PM!");
            }
            if (appointmentRepo.CheckBooking(createAppointmentDto.VehicleId, createAppointmentDto.Date, duration))
            {
                throw new Exception("The vehicle has been booked in this time slot!");
            }

            var appointment = mapper.Map<DAL.Entities.Appointment>(createAppointmentDto);
            await appointmentRepo.CreateAppointment(appointment);
            await hub.Clients.All.SendAsync("AppointmentChanged", new { appointmentId = appointment.AppointmentId, status = appointment.Status, customerId = appointment.CustomerId });
        }

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(int appointmentId)
        {
            var appointment = await appointmentRepo.GetAppointmentById(appointmentId);  
            return mapper.Map<AppointmentDto?>(appointment);
        }

        public async Task<ICollection<AppointmentDto>> GetAppointments()
        {
            var appointments = await appointmentRepo.GetAppointments();
            return mapper.Map<ICollection<AppointmentDto>>(appointments);
        }

        public async Task<ICollection<AppointmentDto>> GetAppointmentsByCustomerIdAsync(int customerId)
        {
            var appointments = await appointmentRepo.GetAppointmentsByCustomerId(customerId);
            return mapper.Map<ICollection<AppointmentDto>>(appointments);
        }

        public async Task<ICollection<AppointmentDto>> GetAppointmentsByTechnician(int technicianId)
        {
            var appointments = await appointmentRepo.GetAppointmentsByTechnician(technicianId);
            return mapper.Map<ICollection<AppointmentDto>>(appointments);
        }

        public async Task UpdateAppointment(AppointmentDto appointmentDto)
        {
            await appointmentRepo.UpdateAppointment(mapper.Map<DAL.Entities.Appointment>(appointmentDto));
            await hub.Clients.All.SendAsync("AppointmentChanged", new { appointmentId = appointmentDto.AppointmentId, status = appointmentDto.Status, customerId = appointmentDto.CustomerId });
        }

        public async Task CompleteAppointmentAsync(int appointmentId, IDictionary<int, int> actualReplaceQuantities, string? note)
        {
            await appointmentRepo.CompleteAppointmentAsync(appointmentId, actualReplaceQuantities, note);
            var ap = await appointmentRepo.GetAppointmentById(appointmentId);
            if (ap != null)
                await hub.Clients.All.SendAsync("AppointmentChanged", new { appointmentId = ap.AppointmentId, status = ap.Status, customerId = ap.CustomerId });
        }

        public async Task CancelAppointmentAsync(int appointmentId, string? note)
        {
            await appointmentRepo.CancelAppointmentAsync(appointmentId, note);
            var ap = await appointmentRepo.GetAppointmentById(appointmentId);
            if (ap != null)
                await hub.Clients.All.SendAsync("AppointmentChanged", new { appointmentId = ap.AppointmentId, status = ap.Status, customerId = ap.CustomerId });
        }

        public async Task<int> AutoMarkInProgressAsync()
        {
            var count = await appointmentRepo.AutoMarkInProgressAsync(DateTime.Now);
            if (count > 0)
                await hub.Clients.All.SendAsync("AppointmentsBulkChanged");
            return count;
        }

        public async Task DeleteAppointmentAsync(int appointmentId)
        {
            var ap = await appointmentRepo.GetAppointmentById(appointmentId);
            if (ap != null)
            {
                await appointmentRepo.DeleteAppointmentAsync(appointmentId);
                await hub.Clients.All.SendAsync("AppointmentDeleted", new { appointmentId = ap.AppointmentId, customerId = ap.CustomerId });
            }
        }

        public async Task DeletePendingAppointmentAsync(int appointmentId)
        {
            var ap = await appointmentRepo.GetAppointmentById(appointmentId);
            if (ap != null)
            {
                await appointmentRepo.DeletePendingAppointmentAsync(appointmentId);
                await hub.Clients.All.SendAsync("AppointmentDeleted", new { appointmentId = ap.AppointmentId, customerId = ap.CustomerId });
            }
        }

        public Task MarkPaymentPaidAsync(int appointmentId) => appointmentRepo.MarkPaymentPaidAsync(appointmentId);
    }
}
