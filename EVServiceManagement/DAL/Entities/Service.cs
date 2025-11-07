namespace DAL.Entities;

public partial class Service
{
    public int ServiceId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Price { get; set; }

    public string Type { get; set; } = null!;

    public int Duration { get; set; }

    public decimal? Km { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<ServiceOrderDetail> ServiceOrderDetails { get; set; } = new List<ServiceOrderDetail>();

    public virtual ICollection<ServicePart> ServiceParts { get; set; } = new List<ServicePart>();
}
