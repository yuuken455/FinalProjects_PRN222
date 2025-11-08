namespace DAL.Entities;

public partial class Part
{
    public int PartId { get; set; }

    public string Name { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int StockQuantity { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<PartRequest> PartRequests { get; set; } = new List<PartRequest>();

    public virtual ICollection<ServiceOrderDetail> ServiceOrderDetails { get; set; } = new List<ServiceOrderDetail>();

    public virtual ICollection<ServicePart> ServiceParts { get; set; } = new List<ServicePart>();
}
