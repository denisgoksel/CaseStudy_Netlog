namespace CaseStudy_Netlog.Core.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }
    }
}
