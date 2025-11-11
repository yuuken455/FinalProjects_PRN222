using System;
using System.Collections.Generic;

namespace DAL.TempEntities;

public partial class PaymentDetail
{
    public int PaymentDetailId { get; set; }

    public int PaymentId { get; set; }

    public string Method { get; set; } = null!;

    public decimal Amount { get; set; }

    public string? TransactionCode { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string Status { get; set; } = null!;

    public string? Note { get; set; }

    public virtual Payment Payment { get; set; } = null!;
}
