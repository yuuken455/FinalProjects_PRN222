using DAL.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class PartRepo : IPartRepo
    {
        private readonly EVServiceManagementContext dbContext;

        public PartRepo(EVServiceManagementContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ICollection<Part>> GetAll()
        {
            return await dbContext.Parts.ToListAsync();
        }

        public async Task<Part?> GetPartById(int id)
        {
            return await dbContext.Parts.FirstOrDefaultAsync(p => p.PartId == id);  
        }
    }
}
