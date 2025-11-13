using AutoMapper;
using BLL.DTOs.AccountDtos;
using BLL.IService;
using DAL.IRepository;

namespace BLL.Service
{
    public class TechnicianService : ITechnicianService
    {
        private readonly IMapper mapper;
        private readonly ITechnicianRepo technicianRepo;

        public TechnicianService(IMapper mapper,ITechnicianRepo technicianRepo)
        {
            this.mapper = mapper;
            this.technicianRepo = technicianRepo;
        }

        public async Task<ICollection<TechnicianDto>> GetTechniciansCanWork(DateTime startTime, int duration)
        {
            var free = await technicianRepo.GetTechniciansCanWork(startTime, duration);

            return mapper.Map<ICollection<TechnicianDto>>(free);    
        }
    }
}
