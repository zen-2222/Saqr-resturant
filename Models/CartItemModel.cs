namespace SaqrResturant.Models
{
    public class CartItemModel
    {
        public int MenuID { get; set; }
        public int Quantity { get; set; } = 0;


        public string? name { get; set; }
        public double? Price { get; set; }
        public string? Imgpath { get; set; }
    }
}
