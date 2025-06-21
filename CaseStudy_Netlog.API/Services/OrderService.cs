using CaseStudy_Netlog.Core.Interfaces;
using CaseStudy_Netlog.Data.Context;
using System.Threading.Tasks;

namespace CaseStudy_Netlog.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, int newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.Status != 0) return false;

            order.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
