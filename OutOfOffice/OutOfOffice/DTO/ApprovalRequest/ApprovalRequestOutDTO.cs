using OutOfOffice.DTO.Employee;
using OutOfOffice.DTO.LeaveRequest;
using System.ComponentModel;

namespace OutOfOffice.DTO.ApprovalRequest
{
    public class ApprovalRequestOutDTO
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }

        [DisplayName("Comment")]
        public string? Comment { get; set; }

        [DisplayName("Approved by")]
        public virtual EmployeeOutDTO Approver { get; set; } = null!;

        [DisplayName("Leave request")]
        public virtual LeaveRequestOutDTO LeaveRequest { get; set; } = null!;
    }
}
