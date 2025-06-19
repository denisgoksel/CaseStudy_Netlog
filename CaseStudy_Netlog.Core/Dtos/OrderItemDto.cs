using System.Runtime.Serialization;

[DataContract(Namespace = "http://tempuri.org/")]
public class OrderItemDto
{
    [DataMember(Order = 1)]
    public string ProductName { get; set; }

    [DataMember(Order = 2)]
    public int Quantity { get; set; }
}
