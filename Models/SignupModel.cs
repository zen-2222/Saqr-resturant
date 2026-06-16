using System.ComponentModel.DataAnnotations;

namespace SaqrResturant.Models
{
    public class SignupModel
    {
        [Required(ErrorMessage ="Please Enter your username")]
        public string userName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please Enter your full name")]

        public string fullName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please Enter your password")]

        public string password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please Enter your password confirmation")]

        public string Confpassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please Enter your phone number")]

        public string phoneNumber { get; set; } = string.Empty;
        public string? deliveryLocation { get; set; } = string.Empty;


    }
}
