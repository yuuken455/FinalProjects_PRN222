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

        public AppointmentService(IMapper mapper, IAppointmentRepo appointmentRepo)
        {
            this.mapper = mapper;
            this.appointmentRepo = appointmentRepo;
        }

        public async Task<ICollection<AppointmentDto>> GetAppointmentsByCustomerIdAsync(int customerId)
        {
            var appointments = await appointmentRepo.GetAppointmentsByCustomerId(customerId);
            return mapper.Map<ICollection<AppointmentDto>>(appointments);
        }
    }
}
