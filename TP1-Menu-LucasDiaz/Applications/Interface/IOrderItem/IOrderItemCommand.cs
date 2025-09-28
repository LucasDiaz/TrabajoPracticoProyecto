using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interface.IOrderItem
{
    public interface IOrderItemCommand
    {
        Task InsertOrderItem(OrderItem orderItem);
        Task InsertOrderItemRange(List<OrderItem> orderItems);
        Task UpdateOrderItem(OrderItem orderItem);
        Task RemoveOrderItem(IEnumerable<OrderItem> orderItem);

       
    }
}
