using Applications.Exceptions;
using Applications.Interface.DeliveryType;
using Applications.Interface.Dish;
using Applications.Interface.IOrderItem;
using Applications.Interface.Order;
using Applications.Interface.Order.IOrder;
using Applications.Models.Request;
using Applications.Models.Response;
using Azure;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.Order
{
    public class OrderCreate : IOrderCreate
    {
        private readonly IOrderCommand _command;
        private readonly IDeliveryTypeQuery _deliveryTypeQuery;
        private readonly IOrderQuery _query; //excepcion de si exite la orden
        private readonly IDishQuery _dishQuery;
        private readonly IOrderItemQuery _orderItemQuery; //excepcion de si exite el item 
        private readonly IOrderItemCommand _orderItemCommand;

        public OrderCreate(IOrderCommand command, IDeliveryTypeQuery deliveryTypeQuery, IOrderQuery query, IDishQuery dishQuery, IOrderItemQuery orderItemQuery, IOrderItemCommand orderItemCommand)
        {
            _command = command;
            _deliveryTypeQuery = deliveryTypeQuery;
            _query = query;
            _dishQuery = dishQuery;
            _orderItemQuery = orderItemQuery;
            _orderItemCommand = orderItemCommand;
        }

        public async Task<OrderCreateResponse?> CreateOrder(OrderRequest orderRequest)
        {
            if (orderRequest.Delivery == null || orderRequest.Delivery.id <= 0)
            {
                throw new MissingDeliveryTypeException();
            }


            //crear order
            var deliveryType = await _deliveryTypeQuery.GetDeliveryTypeById(orderRequest.Delivery.id);

            if (deliveryType == null)
            {
                throw new MissingDeliveryTypeException();
            }

            var order = new Domain.Entities.Order
            {
                DeliveryTypeId = orderRequest.Delivery.id,
                Price = 0,
                StatusId = 1,
                DeliveryTo = orderRequest.Delivery.to,
                Notes = orderRequest.Notes,
                UpdateDate = DateTime.Now,
                CreateDate = DateTime.Now
            };
            //guardar order
            await _command.InsertOrder(order);
            //crear orderItem
            var listItems = orderRequest.Items;
            foreach (var item in listItems)
            {
                var dish = await _dishQuery.GetDishById(item.Id);
                if (dish == null)
                {
                    throw new InvalidDishException(item.Id);
                }
                if (item.quantity <= 0)
                {
                    throw new InvalidQuantityException(item.Id);
                }
            }

            var listorderItems = listItems.Select(item => new OrderItem
            {
                DishId = item.Id,
                Quantity = item.quantity,
                Notes = item.notes,
                StatusId = 1,
                OrderId = order.OrderId,
            }).ToList();
            order.Price = await CalculateTotalPrice(listItems);
            await _orderItemCommand.InsertOrderItemRange(listorderItems);
            await _command.UpdateOrder(order);
            //relacionar orderItem con dish

            return new OrderCreateResponse
            {
                orderNumber = (int)order.OrderId,// se genera auto, ver
                totalAmount = (double)order.Price,
                createdAt = DateTime.Now
            };

        }
        private async Task<decimal> CalculateTotalPrice(List<ItemRequest> orderItems)
        {
            decimal total = 0;
            //dish obtener
            //ver exita el plato y que el valor sea mayor a cero 

            foreach (var item in orderItems)
            {
                var dish = await _dishQuery.GetDishById(item.Id);
                if (dish == null)
                {
                    throw new InvalidDishException(item.Id);
                }
                if (item.quantity <= 0)
                {
                    throw new InvalidQuantityException(item.Id);
                }
                total += dish.Price * item.quantity;
            }
            return total;
        }
    }
}
