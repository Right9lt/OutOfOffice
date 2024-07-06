using OutOfOffice.DTO.Employee;
using OutOfOffice.Entities;
using System.ComponentModel;

namespace OutOfOffice.DTO.LeaveRequest
{
    public class LeaveRequestOutDTO
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("Absense reason")]
        public int AbsenseReason { get; set; }

        [DisplayName("Start date")]
        public DateOnly StartDate { get; set; }

        [DisplayName("End date")]
        public DateOnly EndDate { get; set; }

        [DisplayName("Comment")]
        public string? Comment { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }

        [DisplayName("Created by")]
        public EmployeeOutDTO Employee { get; set; } = null!;
    }
}
