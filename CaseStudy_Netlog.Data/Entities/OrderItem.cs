using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CaseStudy_Netlog.Data.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Required, MaxLength(255)]
        public string ProductName { get; set; }

        [Required]
        public int Quantity { get; set; }

        public Order Order { get; set; }
    }
}
