using Applications.Interface.IOrderItem;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Query
{
    public class OrderItemQuery : IOrderItemQuery
    {
        private readonly AppDbContext _context;
        public OrderItemQuery(AppDbContext context)
        {
            _context = context;
        }
        public async Task<OrderItem?> GetOrderItemById(int id)
        {
            return await _context.OrderItems.FindAsync(id).AsTask();
        }
        public async Task<List<OrderItem>> GetAllOrderItems()
        {
            return await _context.OrderItems.ToListAsync();
        }
        public async Task<bool> ExistsByDishId(Guid dishId)
        {
            var inactiveStatuses = new[] { 4, 5 };

            return await _context.OrderItems
                .AnyAsync(item =>
                    item.DishId == dishId &&
                    !inactiveStatuses.Contains(item.Order.StatusId)
                );
        }

        public Task<OrderItem?> GetOrderItemById(long id)
        {
            throw new NotImplementedException();
        }
    }
}
