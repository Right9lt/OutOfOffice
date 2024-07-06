using System.ComponentModel;

namespace OutOfOffice.DTO.Employee
{
    public class PeoplePartnerDTO
    {

        [DisplayName("People partner fullname")]
        
        public string Fullname { get; set; } = null!;

        [DisplayName("People partner email")]
        public string Email { get; set; } = null!;
        
    }
}
