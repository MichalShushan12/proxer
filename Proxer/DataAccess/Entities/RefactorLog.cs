using System;
using System.Collections.Generic;

namespace DataAccess.Entities;

public partial class RefactorLog
{
    public int RefactorLogId { get; set; }

    public int UserId { get; set; }

    public string OriginalCode { get; set; } = null!;

    public string? CorrectedCode { get; set; }

    public string ActionType { get; set; } = null!;

    public DateTime RequestDate { get; set; }

    public virtual User User { get; set; } = null!;
}
