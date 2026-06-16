using System.ComponentModel.DataAnnotations;

namespace SaqrResturant.Models
{
    public enum Categories {Burgers,Pizza,Drinks,Desserts,Others}
    public class menuModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please enter the name of the item")]
        public string? name { get; set; }
        public string description { get; set; } = "";
        [Required(ErrorMessage ="Please enter the price")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid item price")]
        public double? price { get; set; }
        [Required(ErrorMessage ="Please enter a valid category")]
        public Categories category { get; set; }
        public string? Imgpath { get; set; } = string.Empty;
        public ICollection<OrderDetailsModel> OrderDetails { get; set; } = new List<OrderDetailsModel>();
    }
}
