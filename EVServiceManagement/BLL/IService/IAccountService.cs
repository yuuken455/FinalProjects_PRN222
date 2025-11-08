using BLL.DTOs.AccountDtos;

namespace BLL.IService
{
    public interface IAccountService
    {
        Task<AccountDto?> LoginAsync(string email, string password);
    }
}
