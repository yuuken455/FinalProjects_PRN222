using BLL.DTOs.PartDtos;

namespace BLL.IService
{
    public interface IPartRequestService
    {
        Task<int> CreateAsync(CreatePartRequestDto dto);               // Staff
        Task ApproveAsync(ApprovePartRequestDto dto);                  // Manager
        Task ReceiveAsync(ReceivePartRequestDto dto);                  // Staff
        Task<PartRequestDto?> GetAsync(int id);
        Task<List<PartRequestDto>> ListAsync(string? status = null, int? staffId = null);
    }
}
