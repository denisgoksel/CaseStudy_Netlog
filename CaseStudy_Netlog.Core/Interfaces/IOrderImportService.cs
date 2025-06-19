
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseStudy_Netlog.Core.Interfaces
{
    public interface IOrderImportService
    {
        Task<List<OrderDto>> GetOrdersFromSoapAsync(DateTime date);
        Task SaveOrdersAsync(List<OrderDto> orders);
        Task SendDeliveredOrdersToRestApiAsync();
    }
}
