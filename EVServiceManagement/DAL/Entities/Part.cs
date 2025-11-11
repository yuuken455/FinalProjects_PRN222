using System;
using System.Collections.Generic;

namespace DAL.Entities;

public class Part
{
    public int PartId { get; set; }
    public string Name { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public string Status { get; set; } = "Active";

    public virtual ICollection<ServicePart> ServiceParts { get; set; } = new List<ServicePart>();
    public virtual ICollection<PartRequest> PartRequests { get; set; } = new List<PartRequest>();
}
