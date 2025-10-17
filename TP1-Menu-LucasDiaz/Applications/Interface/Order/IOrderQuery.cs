using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interface.Order
{
    public interface IOrderQuery
    {
        Task<Domain.Entities.Order?> GetOrderById(long id);
        Task<IEnumerable<Domain.Entities.Order?>> GetOrderWithFilter(int? statusId, DateTime? from, DateTime? to);
        Task<List<Domain.Entities.Order>> GetAllOrders();

        Task<Domain.Entities.Order?> GetOrderItemsById(long id);
    }
}
