using System;

namespace CaseStudy_Netlog.Core.Entities
{
    public class Delivery
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string PlateNumber { get; set; }

        public string DeliveredBy { get; set; }
    }
}
