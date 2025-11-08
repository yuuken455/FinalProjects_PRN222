namespace BLL.DTOs.AccountDtos
{
    public class AccountDto
    {
        public int AccountId { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string? Address { get; set; }

        public string Status { get; set; } = null!;

        public virtual CustomerDto? CustomerDto { get; set; }

        public virtual ManagerDto? ManagerDto { get; set; }

        public virtual StaffDto? StaffDto { get; set; }

        public virtual TechnicianDto? TechnicianDto { get; set; }
    }
}
