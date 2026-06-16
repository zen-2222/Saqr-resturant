using System.ComponentModel.DataAnnotations;

namespace SaqrResturant.Models
{
    public class ChangepasswordModel
    {
        [Required(ErrorMessage ="Please enter your old password")]
        public string? currentpassword {  get; set; }
        [Required(ErrorMessage ="Please enter your new password")]
        public string? newpassword { get; set; }
        [Required(ErrorMessage = "Please confirm your new password")]
        [Compare("newpassword",ErrorMessage ="Old password must be the same as the new password")]
        public string? confirmnewpassword { get; set; }

        public int? id { get; set; }

    }
}
