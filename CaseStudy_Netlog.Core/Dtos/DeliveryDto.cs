using System;
using System.Runtime.Serialization;

[DataContract(Namespace = "http://tempuri.org/")]
public class DeliveryDto
{
    [DataMember(Order = 1)]
    public int OrderId { get; set; }

    [DataMember(Order = 2)]
    public DateTime DeliveryDate { get; set; }

    [DataMember(Order = 3)]
    public string PlateNumber { get; set; }

    [DataMember(Order = 4)]
    public string DeliveredBy { get; set; }
}
