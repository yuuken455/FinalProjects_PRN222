namespace BLL.DTOs.PartDtos
{
    public class CreatePartRequestDto
    {
        public int RequestedBy { get; set; }

        public int? ApprovedBy { get; set; }

        public int? PartId { get; set; }

        public int Quantity { get; set; }

        public string? Notes { get; set; }
    }
}
