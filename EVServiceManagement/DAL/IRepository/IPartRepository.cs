using DAL.Entities;

namespace DAL.IRepository
{
    public interface IPartRepository
    {
        Task<Part?> GetByIdAsync(int id);
        Task<List<Part>> GetAllAsync(string? keyword = null);
        Task AddAsync(Part part);
        Task UpdateAsync(Part part);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task AdjustStockAsync(int partId, int delta); // + tăng, - giảm
    }
}
