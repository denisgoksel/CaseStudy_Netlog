using CaseStudy_Netlog.Data.Context;
using CaseStudy_Netlog.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudy_Netlog.Data.Controller
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersByStatusAsync(int status);
        Task UpdateOrderAsync(Order order);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(int status)
        {
            return await _context.Orders
                .Include(o => o.Delivery)
                .Where(o => o.Status == status)
                .ToListAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }

}
