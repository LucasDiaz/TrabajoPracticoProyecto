using Applications.Exceptions;
using Applications.Interface.Dish;
using Applications.Interface.IOrderItem;
using Applications.Interface.Order;
using Applications.Interface.Order.IOrder;
using Applications.Models.Request;
using Applications.Models.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.Order
{
    public class UpdateItemFromOrder: IUpdateItemFromOrder
    {
        private readonly IOrderCommand _command;
        private readonly IOrderQuery _query;
        private readonly IOrderItemQuery _OrderItemQuery;
        private readonly IOrderItemCommand _OrderItemCommand;
        private readonly IDishQuery _DishQuery;

        public UpdateItemFromOrder(IOrderCommand command, IOrderQuery query, IOrderItemQuery orderItemQuery, IOrderItemCommand OrderItemCommand, IDishQuery dishQuery)
        {
            _command = command;
            _query = query;
            _OrderItemQuery = orderItemQuery;
            _OrderItemCommand = OrderItemCommand;
            _DishQuery = dishQuery;
        }

        public async Task<OrderUpdateReponse> UpdateItemQuantity(long orderId, OrderUpdateRequest listItems)
        {
            //1. buscar la orden por id
            var order = await _query.GetOrderById(orderId);
            if (order == null)
            {
                // **Excepción 404: Orden no encontrada**
                throw new OrderNotFoundException(orderId);
            }

            //2. no se puede modificar si no está 'Pending'
            if (order.OverallStatus.Id != 1)
            {
                // **Excepción 400: Estado no modificable**
                string currentStatusName = order.OverallStatus?.Name ?? "Desconocido";
                throw new InvalidOrderStatusException(currentStatusName);
            }
            //3. borrar todos los items de la orden
            if (listItems.items == null || !listItems.items.Any())
            {
                // **Excepción 400: Lista de ítems vacía**
                throw new Applications.Exceptions.InvalidOperationException("La orden debe contener al menos un plato.");
            }
            await _OrderItemCommand.RemoveOrderItem(order.OrderItems);
            //4. crear los nuevos items
            var newOrderItems = listItems.items.Select(item => new OrderItem
            {
                OrderId = orderId,
                DishId = item.Id,
                Quantity = item.quantity,
                Notes = item.notes,
                StatusId = 1
            }).ToList();
            //5. Insertar los nuevos items
            await _OrderItemCommand.InsertOrderItemRange(newOrderItems);
            //6. recalcular el precio total
            decimal totalPrice = 0;
            foreach (var item in newOrderItems)
            {
                var dish = await _DishQuery.GetDishById(item.DishId);
                //var dish = await _dishQuery.GetDishById(item.Id);
                if (dish == null || !dish.Available)
                {   // **Excepción 400: Plato no válido**
                    throw new InvalidDishException(item.DishId);
                }
                if (item.Quantity <= 0)
                {// **Excepción 400: Plato no válido**
                    throw new InvalidQuantityException(item.DishId);
                }
 
                    totalPrice += dish.Price * item.Quantity;
               
            }
            //7. actualizar la orden
            order.Price = totalPrice;
            order.UpdateDate = DateTime.Now;
            await _command.UpdateOrder(order);
            //8. retornar la respuesta
            return new OrderUpdateReponse
            {
                orderNumber = (int)order.OrderId,
                totalAmount = (double)order.Price,
                UpdateAt = order.UpdateDate
            };
        }
    }
}
