using System.ComponentModel.DataAnnotations;

namespace SaqrResturant.Models
{
    public enum Paymentmethod{ Cash,Credit}
    public class PaymentModel
    {
        [Required(ErrorMessage ="Please enter your full name")]
        public string? fullname { get; set; }
        [Required(ErrorMessage = "Please enter your phone number")]
        
        public string? phonenumber { get; set; }
        [Required(ErrorMessage = "Please enter your payment method of choice")]

        public Paymentmethod paymentmethod { get; set; }

        public string? Cardnumber { get; set; } 
        public string? Cardholdername { get; set; }
        public DateOnly? expirydate { get; set; }
        public string? cvv {  get; set; }
        [Required(ErrorMessage = "Please enter your delivery location")]
        public string? location { get; set; }
        public double? total { get; set; } = 0;
        public string? Fullname { get; set; }

    }
}
