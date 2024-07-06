using OutOfOffice.Entities;
using OutOfOffice.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.DTO.LeaveRequest
{
    public class LeaveRequestDTO
    {
        [Required]
        [DisplayName("Absense reason")]
        public int AbsenseReason { get; set; }

        [Required]
        [DisplayName("Start date")]
        [DataType(DataType.Date)]
        [DateLessThan(nameof(EndDate))]
        [GreaterThanNow]
        public DateOnly StartDate { get; set; }

        [Required]
        [DisplayName("End date")]
        [DataType(DataType.Date)]
        [DateGreaterThan(nameof(StartDate))]
        public DateOnly EndDate { get; set; }

        [DisplayName("Comment")]
        [DataType(DataType.Text)]
        public string? Comment { get; set; }

    }
}
