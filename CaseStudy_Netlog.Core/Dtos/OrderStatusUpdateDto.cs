using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudy_Netlog.Core.Dtos
{
    public class OrderStatusUpdateDto
    {
        public int OrderId { get; set; }
        public int NewStatus { get; set; } //  1 = Teslim Edildi
    }
}
