
using EfeTest.Backend.Domain.Enums;
using System.ComponentModel.DataAnnotations;


namespace EfeTest.Backend.Domain.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string ShippingAddress { get; set; }
        public double TotalAmount { get; set; }
        public User User { get; set; }
        public List<OrderItem> Items { get; set; }

    }
}
