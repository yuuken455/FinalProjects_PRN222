using DAL.Entities;

namespace DAL.IRepository
{
    public interface IPartRequestRepository
    {
        Task<PartRequest?> GetByIdAsync(int id, bool includeNav = true);
        Task AddAsync(PartRequest req);
        Task UpdateAsync(PartRequest req);
        Task<List<PartRequest>> ListAsync(string? status = null, int? staffId = null);
    }
}
