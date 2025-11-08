using AutoMapper;
using BLL.DTOs.ServiceDtos;
using BLL.IService;
using DAL.IRepository;

namespace BLL.Service
{
    public class ServiceService : IServiceService
    {
        private readonly IMapper mapper;
        private readonly IServiceRepo serviceRepo;

        public ServiceService(IMapper mapper,IServiceRepo serviceRepo)
        {
            this.mapper = mapper;
            this.serviceRepo = serviceRepo;
        }

        public async Task<ICollection<ServiceDto>> GetAllServices()
        {
            var services = await serviceRepo.GetAllServicesAsync();

            return mapper.Map<ICollection<ServiceDto>>(services);   
        }
    }
}
