using DAL.Entities;

namespace DAL.IRepository
{
    public interface IAccountRepo
    {
        Task<Account?> GetAccountByEmailAsync(string email);
        Task<Account?> GetAccountByPhoneAsync(string phone);
    }
}
