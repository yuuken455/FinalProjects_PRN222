namespace BLL.DTOs.PartDtos
{
    public class PartDto
    {
        public int PartId { get; set; }
        public string Name { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public string Status { get; set; } = "Active";
    }
}
