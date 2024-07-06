using OutOfOffice.DTO.Employee;
using System.ComponentModel;


namespace OutOfOffice.DTO.Project
{
    public class ProjectOutDTO
    {
        public int Id { get; set; }

        [DisplayName("Project type")]
        public int ProjectType { get; set; }

        [DisplayName("Start date")]
        public DateOnly StartDate { get; set; }

        [DisplayName("End date")]
        public DateOnly? EndDate { get; set; }

        [DisplayName("Comment")]
        public string? Comment { get; set; }

        [DisplayName("Status")]
        public bool Status { get; set; }

        [DisplayName("Project employees")]
        public ICollection<EmployeeOutDTO> Employees { get; set; }
    }
}
