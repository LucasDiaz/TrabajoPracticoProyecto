using Applications.Interface.Order;
using Applications.Interface.Order.IOrder;
using Applications.Models.Request;
using Applications.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.Order
{
    public class UpdateOrderItemStatus: IUpdateOrderItemStatus
    {
        private readonly IOrderCommand _ordercommand;
        private readonly IOrderQuery _orderQuery;

        public UpdateOrderItemStatus(IOrderCommand orderCommand, IOrderQuery orderQuery)
        {
            _ordercommand = orderCommand;
            _orderQuery = orderQuery;
        }

        public async Task<OrderUpdateReponse> UpdateItemStatus(long orderId, int itemId, OrderItemUpdateRequest request)
        {
            // 1. Buscar la orden
            var order = await _orderQuery.GetOrderById(orderId);
            if (order == null)
                throw new Exception("Order not found");

            // 2. Buscar el item dentro de la orden
            var item = order.OrderItems.FirstOrDefault(i => i.OrderItemId == itemId);
            if (item == null)
                throw new Exception("Item not found in the order");

            // 3. Actualizar estado del ítem
            item.StatusId = request.status;

            // 4. Actualizar estado general de la orden
            UpdateOrderStatus(order);

            // 5. Persistir cambios
            await _ordercommand.UpdateOrder(order);

            // 6. Devolver respuesta
            return new OrderUpdateReponse
            {
                orderNumber = (int)order.OrderId,
                totalAmount = (double)order.Price,
                UpdateAt = DateTime.UtcNow
            };
        }

        private void UpdateOrderStatus(Domain.Entities.Order order)
        {
            // Si todos los ítems están Ready -> orden Ready
            if (order.OrderItems.All(i => i.StatusId == 3))
            {
                order.StatusId = 3;
            }
            // Si al menos uno está In Progress -> orden In Progress
            else if (order.OrderItems.Any(i => i.StatusId == 2))
            {
                order.StatusId = 2;
            }
            // Si al menos uno está Delivery -> orden Delivery
            else if (order.OrderItems.Any(i => i.StatusId == 4))
            {
                order.StatusId = 4;
            }
            // Si todos los ítems están Closed -> orden Closed
            else if (order.OrderItems.All(i => i.StatusId == 5))
            {
                order.StatusId = 5;
            }
            // Caso inicial -> Pending
            else
            {
                order.StatusId = 1;
            }
        }
    }
}
