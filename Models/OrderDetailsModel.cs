using System.ComponentModel.DataAnnotations;

namespace SaqrResturant.Models
{
    public class OrderDetailsModel
    {
        public int? Id { get; set; } // setting composite key is difficult
        public int? OrderId { get; set; } //FK
        public orderModel Order { get; set; } = null!;
        [Required(ErrorMessage = "Please enter the item ID")]
        public int? MenuId { get; set; } //FK
        public menuModel Menu { get; set; } = null!;
        [Required(ErrorMessage ="Please enter quantity")]
        [Range(1,int.MaxValue,ErrorMessage = "Please enter valid quantity")]
        public int? quantity { get; set; }

        [Required(ErrorMessage = "Please enter an item price")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid item price")]
        public double? itemPrice { get; set; }

    }
}
