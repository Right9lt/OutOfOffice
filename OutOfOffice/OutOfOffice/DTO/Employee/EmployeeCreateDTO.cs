using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.DTO.Employee
{
    public class EmployeeCreateDTO
    {
        [Required]
        [DisplayName("Fullname")]
        public string Fullname { get; set; } = null!;

        [Required]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

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

        [DisplayName("Out of office balance")]
        [Required]
        public int OutOfOfficeBalance { get; set; }

        [DisplayName("People partner")]
        public int? PeoplePartnerId { get; set; } = null;
    }
}
