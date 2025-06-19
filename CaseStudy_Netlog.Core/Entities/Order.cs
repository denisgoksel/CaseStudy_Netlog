using System;
using System.Collections.Generic;
using CaseStudy_Netlog.Core.Enums;

namespace CaseStudy_Netlog.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string DeliveryPoint { get; set; }

        public string ReceiverName { get; set; }

        public string ContactPhone { get; set; }

        public OrderStatus Status { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Delivery Delivery { get; set; }
    }
}
