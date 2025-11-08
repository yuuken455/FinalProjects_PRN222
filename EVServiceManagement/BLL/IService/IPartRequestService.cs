using BLL.DTOs.PartDtos;

namespace BLL.IService
{
    public interface IPartRequestService
    {
        Task<ICollection<PartRequestDto>> GetPartRequestByStaffId(int staffId);
        Task AddPartRequest(CreatePartRequestDto createPartRequestDto);
    }
}
