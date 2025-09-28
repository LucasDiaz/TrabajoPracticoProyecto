using Applications.Models.Request;
using Applications.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interface.Order.IOrder
{
    public interface IUpdateItemFromOrder
    {
        Task<OrderUpdateReponse> UpdateItemQuantity(long orderId, OrderUpdateRequest listItems);
    }
}
