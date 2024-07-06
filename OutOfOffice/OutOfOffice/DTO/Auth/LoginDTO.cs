using System.ComponentModel.DataAnnotations;

namespace OutOfOffice.DTO.Auth
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="Email field can't be empty")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password field can't be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
