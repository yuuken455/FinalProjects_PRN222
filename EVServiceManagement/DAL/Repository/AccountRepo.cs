using DAL.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class AccountRepo : IAccountRepo
    {
        private readonly EVServiceManagementContext dbContext;

        public AccountRepo(EVServiceManagementContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await dbContext.Accounts
                .Include(a => a.Customer)
                .Include(a => a.Manager)
                .Include(a => a.Staff)
                .Include(a => a.Technician)
                .FirstOrDefaultAsync(a => a.Email == email);
        }
        }
}
