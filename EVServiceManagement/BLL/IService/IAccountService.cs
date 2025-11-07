using BLL.DTOs.Account;

namespace BLL.IService
{
    public interface IAccountService
    {
        Task<AccountDto?> LoginAsync(string email, string password);
    }
}
