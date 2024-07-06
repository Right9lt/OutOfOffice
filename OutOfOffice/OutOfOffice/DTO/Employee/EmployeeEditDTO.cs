using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.DTO.Employee
{
    public class EmployeeEditDTO
    {
        [Required]
        [DisplayName("Fullname")]
        public string Fullname { get; set; } = null!;

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        [DisplayName("Subdivision")]
        public int Subdivision { get; set; }

        [Required]
        [DisplayName("Position")]
        public int Position { get; set; }

        [Required]
        [DisplayName("Out of office balance")]
        public int OutOfOfficeBalance { get; set; }

        [DisplayName("People partner")]
        public virtual int? PeoplePartnerId { get; set; }
    }
}
