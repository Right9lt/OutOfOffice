using System;
using System.Collections.Generic;

namespace OutOfOffice.Entities;

public partial class Project
{
    public int Id { get; set; }

    public int ProjectType { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Comment { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
