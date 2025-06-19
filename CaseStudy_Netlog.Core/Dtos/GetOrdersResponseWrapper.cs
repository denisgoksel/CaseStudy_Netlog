 
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CaseStudy_Netlog.API.Services
{
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class SoapEnvelope<T>
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public SoapBody<T> Body { get; set; }
    }

    public class SoapBody<T>
    {
        [XmlElement(ElementName = "GetOrdersResponse", Namespace = "http://tempuri.org/")]
        public T Response { get; set; }
    }

    [XmlRoot(ElementName = "GetOrdersResponse", Namespace = "http://tempuri.org/")]
    public class GetOrdersResponseWrapper
    {
        [XmlElement(ElementName = "GetOrdersResult")]
        public GetOrdersResult GetOrdersResult { get; set; }
    }

    public class GetOrdersResult
    {
        [XmlElement("OrderDto")]
        public List<OrderDto> Orders { get; set; }
    }

}
