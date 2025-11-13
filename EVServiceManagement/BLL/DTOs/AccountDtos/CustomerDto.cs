namespace BLL.DTOs.AccountDtos
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        // Thêm thông tin tài khoản để lấy tên khách hàng
        public AccountDto? AccountDto { get; set; }
    }
}
