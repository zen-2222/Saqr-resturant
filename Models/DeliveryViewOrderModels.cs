namespace SaqrResturant.Models
{
    public class DeliveryOrderViewModel
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }

        public string CustomerName { get; set; } = "";
        public string Address { get; set; } = "";

        public string Items { get; set; } = "";

        public double TotalPrice { get; set; }

        public Status Status { get; set; }
    }
}
