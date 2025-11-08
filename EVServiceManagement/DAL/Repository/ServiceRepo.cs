using DAL.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class ServiceRepo : IServiceRepo
    {
        private readonly EVServiceManagementContext dbContext;

        public ServiceRepo(EVServiceManagementContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<Service>> GetAllServicesAsync()
        {
            return await dbContext.Services
                .Include(s => s.ServiceParts)
                .ThenInclude(x => x.Part)
                .ToListAsync();
        }
    }
}
