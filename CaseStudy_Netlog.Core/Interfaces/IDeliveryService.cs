 
using System.Threading.Tasks;

namespace CaseStudy_Netlog.Core.Interfaces
{
    public interface IDeliveryService
    {
        Task ProcessDeliveredOrdersAsync();
    }
}
