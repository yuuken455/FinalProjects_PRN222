using BLL.DTOs.PartDtos;

namespace BLL.IService
{
    public interface IPartService
    {
        Task<List<PartDto>> GetAllParts();
        Task<List<PartDto>> GetAllAsync(string? keyword = null);
        Task<PartDto?> GetAsync(int id);
        Task<int> CreateAsync(CreatePartDto dto);
        Task UpdateAsync(UpdatePartDto dto);
        Task DeleteAsync(int id);
    }
}
