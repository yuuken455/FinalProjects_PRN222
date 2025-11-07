using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class PartRequest
{
    public int RequestId { get; set; }

    public int RequestedBy { get; set; }

    public int? ApprovedBy { get; set; }

    public int? PartId { get; set; }

    public int Quantity { get; set; }

    public DateTime? RequestDate { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual Manager? ApprovedByNavigation { get; set; }

    public virtual Part? Part { get; set; }

    public virtual Staff RequestedByNavigation { get; set; } = null!;
}
