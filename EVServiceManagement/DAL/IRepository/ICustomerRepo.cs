using DAL.Entities;

namespace DAL.IRepository
{
    public interface ICustomerRepo
    {
        Task<Customer?> GetCustomerByIdAsync(int customerId);
        Task AddCustomerAsync(Customer customer);

    }
}
