using AutoMapper;
using BLL.DTOs.AppointmentDtos;
using BLL.IService;
using DAL.IRepository;

namespace BLL.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IMapper mapper;
        private readonly IAppointmentRepo appointmentRepo;
        private readonly ICustomerRepo customerRepo;
        private readonly IServiceRepo serviceRepo;

        public AppointmentService(IMapper mapper, IAppointmentRepo appointmentRepo, ICustomerRepo customerRepo, IServiceRepo serviceRepo)
        {
            this.mapper = mapper;
            this.appointmentRepo = appointmentRepo;
            this.customerRepo = customerRepo;
            this.serviceRepo = serviceRepo;
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
                serviceOrderDetailDto.TotalPrice = service.Price;
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

            var appointment = mapper.Map<DAL.Entities.Appointment>(createAppointmentDto);
            await appointmentRepo.CreateAppointment(appointment);
        }

        public async Task<ICollection<AppointmentDto>> GetAppointmentsByCustomerIdAsync(int customerId)
        {
            var appointments = await appointmentRepo.GetAppointmentsByCustomerId(customerId);
            return mapper.Map<ICollection<AppointmentDto>>(appointments);
        }
    }
}
