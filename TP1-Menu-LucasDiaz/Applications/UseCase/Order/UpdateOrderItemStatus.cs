using Applications.Exceptions;
using Applications.Interface.IOrderItem;
using Applications.Interface.Order;
using Applications.Interface.Order.IOrder;
using Applications.Interface.Status;
using Applications.Models.Request;
using Applications.Models.Response;
using Domain.Entities;
using Applications.Enum;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.Order
{
    public class UpdateOrderItemStatus: IUpdateOrderItemStatus
    {
        private readonly IOrderCommand _ordercommand;
        private readonly IOrderQuery _orderQuery;
        private readonly IOrderItemQuery _orderItemQuery;
        private readonly IStatusQuery _statusQuery;

        public UpdateOrderItemStatus(IOrderCommand ordercommand, IOrderQuery orderQuery, IOrderItemQuery orderItemQuery, IStatusQuery statusQuery)
        {
            _ordercommand = ordercommand;
            _orderQuery = orderQuery;
            _orderItemQuery = orderItemQuery;
            _statusQuery = statusQuery;
        }

        public async Task<OrderUpdateReponse> UpdateItemStatus(long orderId, int itemId, OrderItemUpdateRequest request)
        {
            // 1. Buscar la orden
            var order = await _orderQuery.GetOrderById(orderId);
            if (order == null) { 
                //404
            throw new NullException($"Orden con ID {orderId} no encontrada");
            }
        
            // 2. Buscar el item dentro de la orden 
            var item = order.OrderItems.FirstOrDefault(i => i.OrderItemId == itemId);
            if (item == null) {  
                //404
                throw new NullException($"Item no encontrado (ID: {itemId}) en la orden {orderId}"); 
            }

            if (await _statusQuery.StatusExist(request.status))
            {
                // Usamos BadRequestException para el 400 - Estado inválido
                throw new RequeridoException($"No existe el status '{request.status}'");
            }

            if (!IsValidTransition(item.StatusId, request.status))
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
            if (order.OrderItems.All(i => i.StatusId == (int)OrderStatus.Closed))
                order.StatusId = (int)OrderStatus.Closed;
            else if (order.OrderItems.All(i => i.StatusId == (int)OrderStatus.Ready))
                order.StatusId = (int)OrderStatus.Ready;
            else if (order.OrderItems.Any(i => i.StatusId == (int)OrderStatus.InProgress))
                order.StatusId = (int)OrderStatus.InProgress;
            else if (order.OrderItems.Any(i => i.StatusId == (int)OrderStatus.Delivery))
                order.StatusId = (int)OrderStatus.Delivery;
            else
                order.StatusId = (int)OrderStatus.Pending;
        }

        private bool IsValidTransition(int current, int next)
        {
            // Ejemplo de reglas básicas: Pendiente -> En preparación -> Listo -> Entregado
            if (current == (int)OrderStatus.Closed && next != (int)OrderStatus.Closed)
                return false; // no se puede reabrir
            if (current == (int)OrderStatus.Delivery && next == (int)OrderStatus.InProgress)
                return false; // no volver atrás
            return true;
        }
    }
    
}
