using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CaseStudy_Netlog.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required, MaxLength(255)]
        public string DeliveryPoint { get; set; }

        [Required, MaxLength(255)]
        public string ReceiverName { get; set; }

        [Required, MaxLength(50)]
        public string ContactPhone { get; set; }

        [Required]
        public int Status { get; set; }

        // Burada isim OrderItems olmalı, çünkü DbContext'te bu isim kullanılıyor
        public ICollection<OrderItem> OrderItems { get; set; }

        public Delivery Delivery { get; set; }
    }
}
