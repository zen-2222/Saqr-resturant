using System.ComponentModel.DataAnnotations;

namespace SaqrResturant.Models
{
    public enum Status { Pending,Delivering,Delivered,Cancelled}
    public class deliveryModel
    {
        [Required(ErrorMessage ="Please Enter ID")]
        public int? Id { get; set; }
        public int? DeliveryUserId { get; set; }
        public UserModel User { get; set; } = null!;
        [Required(ErrorMessage ="Please enter OrderID")]
        public int? OrderId { get; set; }
        public orderModel Order { get; set; } = null!;
        public DateTime assignedAt { get; set; } = DateTime.Now;
        public DateTime? cancelledAt { get; set; }
        [Required(ErrorMessage ="Please set the status")]
        public Status? status { get; set; }=Status.Pending;

    }
}
