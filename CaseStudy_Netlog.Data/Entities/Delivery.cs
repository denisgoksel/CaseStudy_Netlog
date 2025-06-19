using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseStudy_Netlog.Data.Entities
{
    public class Delivery
    {
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        [Required, MaxLength(50)]
        public string PlateNumber { get; set; }

        [Required, MaxLength(255)]
        public string DeliveredBy { get; set; }

        public Order Order { get; set; }
    }
}
