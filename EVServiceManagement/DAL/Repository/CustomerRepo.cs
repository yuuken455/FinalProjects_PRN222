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

        public async Task AddCustomerAsync(Customer customer)
        {
            await dbContext.Customers.AddAsync(customer);   
            await dbContext.SaveChangesAsync(); 
        }

        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            return await dbContext.Customers
                .Include(x => x.Account)
                .Include(x => x.Vehicles)   
                .FirstOrDefaultAsync(x => x.CustomerId == customerId);
        }
    }
}
