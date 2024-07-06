using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.DTO.ApprovalRequest
{
    public class ApprovalRequestDTO
    {
        [Required]
        [DisplayName("Leave request")]
        public int LeaveRequestId { get; set; }

        [Required]
        [DisplayName("Resolution")]
        public int Status { get; set; }


        [DisplayName("Comment")]
        [DataType(DataType.Text)]
        public string? Comment { get; set; }

    }
}
