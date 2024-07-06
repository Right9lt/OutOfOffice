using OutOfOffice.Entities;
using System.ComponentModel;

namespace OutOfOffice.DTO.Employee
{
    public class EmployeeOutDTO
    {
        [DisplayName("Id")]
        public int Id { get; set; }
        [DisplayName("Fullname")]
        public string Fullname { get; set; } = null!;
        [DisplayName("Email")]
        public string Email { get; set; } = null!;
        [DisplayName("Subdivision")]
        public int Subdivision { get; set; }
        [DisplayName("Position")]
        public int Position { get; set; }
        [DisplayName("Status")]
        public bool Status { get; set; }
        [DisplayName("Balance")]
        public int OutOfOfficeBalance { get; set; }

        [DisplayName("People partner")]
        public PeoplePartnerDTO? PeoplePartner { get; set; }

    }
}
