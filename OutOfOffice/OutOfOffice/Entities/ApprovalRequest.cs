using System;
using System.Collections.Generic;

namespace OutOfOffice.Entities;

public partial class ApprovalRequest
{
    public int Id { get; set; }

    public int ApproverId { get; set; }

    public int LeaveRequestId { get; set; }

    public int Status { get; set; }

    public string? Comment { get; set; }

    public virtual Employee Approver { get; set; } = null!;

    public virtual LeaveRequest LeaveRequest { get; set; } = null!;
}
