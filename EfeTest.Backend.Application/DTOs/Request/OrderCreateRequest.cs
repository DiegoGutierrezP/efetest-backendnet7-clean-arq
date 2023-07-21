using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfeTest.Backend.Application.DTOs.Request
{
    public class OrderCreateRequest
    {
        public int? UserId { get; set; }
        public List<OrderItemCreateRequest> Products { get; set; }
        public string ShippingAddress { get; set; }
    }

    public class OrderItemCreateRequest
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
    }
}
