namespace BLL.DTOs.AccountDtos
{
    public class CreateAccountDto
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string? Address { get; set; }

        public string Status { get; set; } = "Active";
    }
}
