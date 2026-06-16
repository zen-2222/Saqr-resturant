using System.ComponentModel.DataAnnotations;

namespace SaqrResturant.Models
{
    public enum inquiry
    {
        General,
        private_Event,
        complaint
    }
    public class ContactUsModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage ="Please enter your full name")]
        public string? fullname { get; set; } = string.Empty;
        [Required(ErrorMessage ="Please enter your email")]
        public string? email { get; set; }
        [Required(ErrorMessage ="Please enter the subject")]
        public inquiry subject { get; set; }
        [Required(ErrorMessage = "Please enter the Details")]
        public string? Details { get; set; }
        public int? UserId { get; set; } = null;
    
    }
}
