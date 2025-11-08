namespace BLL.DTOs.AccountDtos
{
    public class TechnicianDto
    {
        public int TechnicianId { get; set; }

        public virtual AccountDto AccountDto { get; set; } = null!;
    }
}
