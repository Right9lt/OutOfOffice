using System;
using System.Collections.Generic;

namespace OutOfOffice.Entities;

public partial class LeaveRequest
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int AbsenseReason { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string? Comment { get; set; }

    public int Status { get; set; }

    public virtual ApprovalRequest? ApprovalRequest { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
