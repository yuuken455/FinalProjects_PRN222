using AutoMapper;
using BLL.DTOs.AccountDtos;
using BLL.IService;
using DAL.IRepository;

namespace BLL.Service
{
    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly IAccountRepo accountRepo;
        private readonly ICustomerRepo customerRepo;

        public AccountService(IMapper mapper,IAccountRepo accountRepo, ICustomerRepo customerRepo)
        {
            this.mapper = mapper;
            this.accountRepo = accountRepo;
            this.customerRepo = customerRepo;
        }

        public async Task AddCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            var checkEmail = await accountRepo.GetAccountByEmailAsync(createCustomerDto.CreateAccountDto.Email);
            if (checkEmail != null)
            {
                throw new Exception("Email already exists!");
            }
            var checkPhone = await accountRepo.GetAccountByPhoneAsync(createCustomerDto.CreateAccountDto.Phone);
            if (checkPhone != null)
            {
                throw new Exception("Phone number already exists!");
            }
            var newCustomer = mapper.Map<DAL.Entities.Customer>(createCustomerDto);
            await customerRepo.AddCustomerAsync(newCustomer);
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
