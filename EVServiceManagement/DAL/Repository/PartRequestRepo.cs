using DAL.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class PartRequestRepo : IPartRequestRepo
    {
        private readonly EVServiceManagementContext dbContext;

        public PartRequestRepo(EVServiceManagementContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddPartRequest(PartRequest partRequest)
        {
            await dbContext.PartRequests.AddAsync(partRequest);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<PartRequest>> GetAll()
        {
            return await dbContext.PartRequests
                .Include(pr => pr.RequestedByNavigation)
                .ThenInclude(rp => rp.Account)
                .Include(pr => pr.ApprovedByNavigation)
                .ThenInclude(ap => ap.Account)
                .ToListAsync();
        }
    }
}
