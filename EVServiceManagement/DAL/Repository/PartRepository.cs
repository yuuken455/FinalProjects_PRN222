using DAL.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class PartRepository : IPartRepository
    {
        private readonly EVServiceManagementContext _ctx;
        public PartRepository(EVServiceManagementContext ctx) => _ctx = ctx;

        public Task<Part?> GetByIdAsync(int id) =>
            _ctx.Parts.FirstOrDefaultAsync(x => x.PartId == id);

        public Task<List<Part>> GetAllAsync(string? keyword = null) =>
            _ctx.Parts
                .Where(p => string.IsNullOrEmpty(keyword) || p.Name.Contains(keyword))
                .OrderBy(p => p.Name).ToListAsync();

        public async Task AddAsync(Part part)
        {
            _ctx.Parts.Add(part);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Part part)
        {
            _ctx.Parts.Update(part);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var p = await GetByIdAsync(id);
            if (p != null)
            {
                _ctx.Parts.Remove(p);
                await _ctx.SaveChangesAsync();
            }
        }

        public Task<bool> ExistsAsync(int id) =>
            _ctx.Parts.AnyAsync(x => x.PartId == id);

        public async Task AdjustStockAsync(int partId, int delta)
        {
            var p = await _ctx.Parts.FirstAsync(x => x.PartId == partId);
            var newQty = p.StockQuantity + delta;
            if (newQty < 0) throw new InvalidOperationException("Stock not enough");
            p.StockQuantity = newQty;
            await _ctx.SaveChangesAsync();
        }
    }
}
