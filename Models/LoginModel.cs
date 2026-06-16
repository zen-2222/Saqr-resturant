using System.ComponentModel.DataAnnotations;

namespace SaqrResturant.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter your username")]
        public string userName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your password")]
        public string password { get; set; } = string.Empty;
    }
}
