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
            if (orderRequest.Delivery == null || orderRequest.Delivery.id <= 0 )
            {
                //400
                throw new RequeridoException("Debe especificar un tipo de entrega válido");
            }
            string deliveryTo = orderRequest.Delivery.to;
            int deliveryTypeId = orderRequest.Delivery.id;
            if (string.IsNullOrEmpty(deliveryTo))
            {
                // Aplicar la lógica de negocio para cada ID
                switch (deliveryTypeId)
                {
                    case 1: // Delivery (A Domicilio)
                            // Caso: ({"id": 1, "to": None}, 400) -> Requiere dirección
                        throw new RequeridoException("Para el servicio a domicilio, debe especificar una dirección de entrega válida.");

                    case 2: // Take Away (Para Llevar/Retiro)
                            // Caso: ({"id": 2, "to": ""}, 400) -> Requiere nombre/identificador de cliente
                        throw new RequeridoException("Para retirar, debe especificar un nombre o identificador de cliente.");

                    case 3: // Dine In (Consumo en Sitio/Mesa)
                            // Caso: ({"id": 3, "to": ""}, 400) -> Requiere número de mesa
                        throw new RequeridoException("Para consumo en el sitio, debe especificar un número de mesa.");

                    default:
                        // Fallback para cualquier otro ID de delivery que requiera un destino
                        throw new RequeridoException("El destino de entrega (DeliveryTo) no puede ser vacío para el tipo de entrega seleccionado.");
                }
            }


            //crear order
            var deliveryType = await _deliveryTypeQuery.GetDeliveryTypeById(orderRequest.Delivery.id);

            if (deliveryType == null)
            {
                //400
                throw new RequeridoException("Debe especificar un tipo de entrega válido");
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
                    throw new RequeridoException($"El plato con ID {item.Id} no existe o no está disponible.");
                }
                if (item.quantity <= 0)
                {
                    throw new RequeridoException("La cantidad debe ser mayor a 0");
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
                    throw new RequeridoException($"El plato con ID {item.Id} no existe o no está disponible.");
                }
                if (item.quantity <= 0)
                {
                    throw new RequeridoException("La cantidad debe ser mayor a 0");
                }
                total += dish.Price * item.quantity;
            }
            return total;
        }
    }
}
