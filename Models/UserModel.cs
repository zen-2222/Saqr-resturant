using System.ComponentModel.DataAnnotations;

namespace SaqrResturant.Models
{
    // Enum for roles
    public enum Role {Admin,User,Delivery}

    public class UserModel
    {
        [Required(ErrorMessage = "Please Enter ID")]
        public int? Id { get; set; }
        [Required(ErrorMessage ="Please enter your username")]
        public string userName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter your Full Name")]
        public string? fullName { get; set; }
        [Required(ErrorMessage ="Please enter a password")]
        public string? password { get; set; }
        public string? deliveryLocation { get; set; }
        [Required(ErrorMessage ="Please enter your phone number")]
        public string? phoneNumber { get; set; }
        [Required(ErrorMessage ="Please enter one of the available roles")]
        public Role role { get; set; } = Role.User;
        public DateTime? deletedOn { get; set; } // so when we delete a user, it would still keep their history
        
        public ICollection<orderModel> Orders { get; set; } = new List<orderModel>();
        // for delivery drivers
        public ICollection<deliveryModel> deliveries { get; set; } = null!;

        public ICollection<ContactUsModel> inquiries { get; set; } = null!;
    }
}
