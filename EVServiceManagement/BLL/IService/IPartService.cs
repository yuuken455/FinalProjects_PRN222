using BLL.DTOs.PartDtos;

namespace BLL.IService
{
    public interface IPartService
    {
        Task<ICollection<PartDto>> GetAllParts();
        Task<PartDto?> GetPartById(int id);
    }
}
