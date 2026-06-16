using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaqrResturant.Models
{
    public class orderModel
    {
        [Required(ErrorMessage = "Please Enter ID")]
        public int? Id { get; set; }
        [Required(ErrorMessage ="Please assign a user for the order")]
        public int? UserId { get; set; } // FOREIGN KEY
        public UserModel User { get; set; } = null!; // used for better querying 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required(ErrorMessage ="Please enter a price")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid total price")]
        public double? totalPrice { get; set; } = 0;
        [Required(ErrorMessage ="Please enter the payment method")]
        public Paymentmethod paymentMethod { get; set; }
        public string? location { get; set; }
        public List<OrderDetailsModel> OrderDetails { get; set; } = new List<OrderDetailsModel>();
        public List<deliveryModel> Deliveries { get; set; } = new List<deliveryModel>();
    }
}
