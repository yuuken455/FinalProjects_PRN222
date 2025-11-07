using DAL.Entities;

namespace DAL.IRepository
{
    public interface IAccountRepo
    {
        Task<Account?> GetAccountByEmailAsync(string email);
    }
}
