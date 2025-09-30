using Applications.Exceptions;
using Applications.Interface.Order;
using Applications.Interface.Order.IOrder;
using Applications.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.Order
{
    public class OrderGetById: IOrderGetById
    {
        private readonly IOrderQuery _query;
        public OrderGetById(IOrderQuery orderQuery)
        {
            _query = orderQuery;
        }
        public async Task<OrderDetailsResponse?> GetOrderById(long id)
        {
            var order = await _query.GetOrderById(id);

            if (order == null)
            {
                // Si la orden no se encuentra, lanzamos la excepción.
                throw new OrderNotFoundException(id);
            }

            if (order != null)
            {
                var orderDetails = new OrderDetailsResponse
                {
                    orderNumber = (int)order.OrderId,
                    totalAmount = (double)order.Price,
                    deliveryTo = order.DeliveryTo,
                    notes = order.Notes,
                    status = new GenericResponse { Id = order.StatusId, Name = order.OverallStatus?.Name ?? "Desconocido" },
                    deliveryType = new GenericResponse { Id = order.DeliveryTypeId, Name = order.DeliveryType?.Name ?? "Desconocido" },
                    items = order.OrderItems.Select(item => new OrderItemResponse
                    {
                        Id = 2,
                        Quantity = item.Quantity,
                        notes = item.Dish?.Name,
                        dish = new DishShortResponse { Id = item.DishId, Name = item.Dish?.Name ?? "Desconocido", Image = item.Dish?.ImageUrl ?? "No encontrada" },
                        status = new GenericResponse { Id = item.Status.Id, Name = item.Status?.Name ?? "Desconocido" }
                    }).ToList(),
                    createAt = order.CreateDate,
                    UpdateAt = order.UpdateDate
                };
                return orderDetails;
            }
            ;
            return null;
        }
    }
}
