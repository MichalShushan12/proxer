using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string ApiKey { get; set; } = null!;

    public bool IsPremium { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<RefactorLog> RefactorLogs { get; set; } = new List<RefactorLog>();
}
