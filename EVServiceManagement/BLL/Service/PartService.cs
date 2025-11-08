using AutoMapper;
using BLL.DTOs.PartDtos;
using BLL.IService;
using DAL.IRepository;

namespace BLL.Service
{
    public class PartService : IPartService
    {
        private readonly IMapper mapper;
        private readonly IPartRepo partRepo;

        public PartService(IMapper mapper, IPartRepo partRepo)
        {
            this.mapper = mapper;
            this.partRepo = partRepo;
        }

        public async Task<ICollection<PartDto>> GetAllParts()
        {
            var parts = await partRepo.GetAll();

            return mapper.Map<ICollection<PartDto>>(parts);
        }

        public async Task<PartDto?> GetPartById(int id)
        {
            var part = await partRepo.GetPartById(id);
            return mapper.Map<PartDto?>(part);
        }
    }
}
