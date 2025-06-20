using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudy_Netlog.Core.Interfaces
{
    public interface IIntegrationService
    {
        Task RunDeliveryIntegrationAsync();
    }
}
