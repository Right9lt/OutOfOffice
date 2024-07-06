using OutOfOffice.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.DTO.Project
{
    public class ProjectEditDTO
    {
        [Required]
        [DisplayName("Project type")]
        public int ProjectType { get; set; }

        [Required]
        [DisplayName("Start date")]
        [DataType(DataType.Date)]
        [DateLessThan(nameof(EndDate))]
        [GreaterThanNow]
        public DateOnly StartDate { get; set; }

        
        [DataType(DataType.Date)]
        [DisplayName("End date")]
        [DateGreaterThan(nameof(StartDate))]
        public DateOnly? EndDate { get; set; }

        [DisplayName("Comment")]
        [DataType(DataType.MultilineText)]
        public string? Comment { get; set; }


    }
}
