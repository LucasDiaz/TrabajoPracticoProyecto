using Applications.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interface.Order.IOrder
{
    public interface IOrderGetAllAsync
    {
        Task<IEnumerable<OrderDetailsResponse?>> GetOrderWithFilter(int? statusId, DateTime? from, DateTime? to);
    }
}
