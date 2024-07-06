using System;
using System.Collections.Generic;

namespace OutOfOffice.Entities;

public partial class Employee
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public int Subdivision { get; set; }

    public int Position { get; set; }

    public bool Status { get; set; }

    public int? PeoplePartnerId { get; set; }

    public int OutOfOfficeBalance { get; set; }

    public virtual ICollection<ApprovalRequest> ApprovalRequests { get; set; } = new List<ApprovalRequest>();

    public virtual ICollection<Employee> InversePeoplePartner { get; set; } = new List<Employee>();

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public virtual Employee? PeoplePartner { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
