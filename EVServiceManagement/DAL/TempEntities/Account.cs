using System;
using System.Collections.Generic;

namespace DAL.TempEntities;

public partial class Account
{
    public int AccountId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Address { get; set; }

    public string Status { get; set; } = null!;

    public virtual Customer? Customer { get; set; }

    public virtual Manager? Manager { get; set; }

    public virtual Staff? Staff { get; set; }

    public virtual Technician? Technician { get; set; }
}
