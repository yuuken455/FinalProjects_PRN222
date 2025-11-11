using DAL.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class PartRequestRepository : IPartRequestRepository
    {
        private readonly EVServiceManagementContext _ctx;
        public PartRequestRepository(EVServiceManagementContext ctx) => _ctx = ctx;

        public Task<PartRequest?> GetByIdAsync(int id, bool includeNav = true)
        {
            var q = _ctx.PartRequests.AsQueryable();
            if (includeNav)
                q = q.Include(x => x.Part)
                     .Include(x => x.RequestedByNavigation)
                     .Include(x => x.ApprovedByNavigation);
            return q.FirstOrDefaultAsync(x => x.RequestId == id);
        }

        public async Task AddAsync(PartRequest req)
        {
            _ctx.PartRequests.Add(req);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(PartRequest req)
        {
            _ctx.PartRequests.Update(req);
            await _ctx.SaveChangesAsync();
        }

        public Task<List<PartRequest>> ListAsync(string? status = null, int? staffId = null)
        {
            var q = _ctx.PartRequests
                .Include(x => x.Part)
                .OrderByDescending(x => x.RequestDate)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status)) q = q.Where(x => x.Status == status);
            if (staffId.HasValue) q = q.Where(x => x.RequestedBy == staffId.Value);
            return q.ToListAsync();
        }
    }
}
