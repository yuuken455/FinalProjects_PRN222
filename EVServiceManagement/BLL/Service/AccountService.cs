using AutoMapper;
using BLL.DTOs.Account;
using BLL.IService;
using DAL.IRepository;

namespace BLL.Service
{
    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly IAccountRepo accountRepo;

        public AccountService(IMapper mapper,IAccountRepo accountRepo)
        {
            this.mapper = mapper;
            this.accountRepo = accountRepo;
        }

        public async Task<AccountDto?> LoginAsync(string email, string password)
        {
            var account = await accountRepo.GetAccountByEmailAsync(email);
            if (account == null || account.Password != password)
            {
                return null;
            }

            return mapper.Map<AccountDto>(account);
        }
    }
}
