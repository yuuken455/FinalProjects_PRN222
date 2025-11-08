using AutoMapper;
using BLL.DTOs.PartDtos;
using BLL.IService;
using DAL.IRepository;

namespace BLL.Service
{
    public class PartRequestService : IPartRequestService
    {
        private readonly IMapper mapper;
        private readonly IPartRequestRepo partRequestRepo;

        public PartRequestService(IMapper mapper,IPartRequestRepo partRequestRepo)
        {
            this.mapper = mapper;
            this.partRequestRepo = partRequestRepo;
        }

        public async Task AddPartRequest(CreatePartRequestDto createPartRequestDto)
        {
            if (createPartRequestDto.Quantity < 10)
            {
                throw new Exception("Quantity should be at least 10.");
            }

            var partRequest = mapper.Map<DAL.Entities.PartRequest>(createPartRequestDto);
            partRequest.RequestDate = DateTime.Now;  
            partRequest.Status = "Pending";
            await partRequestRepo.AddPartRequest(partRequest);
        }

        public async Task<ICollection<PartRequestDto>> GetPartRequestByStaffId(int staffId)
        {
            var partRequests = await partRequestRepo.GetAll();   
            var staffPartRequests = partRequests
                .Where(pr => pr.RequestedByNavigation.StaffId == staffId)
                .ToList();

            return mapper.Map<ICollection<PartRequestDto>>(staffPartRequests);
        }
    }
}
