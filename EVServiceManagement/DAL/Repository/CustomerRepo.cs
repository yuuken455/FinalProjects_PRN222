using DAL.Entities;
using DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly EVServiceManagementContext dbContext;

        public CustomerRepo(EVServiceManagementContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            return await dbContext.Customers
                .Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.CustomerId == customerId);
        }
    }
}
