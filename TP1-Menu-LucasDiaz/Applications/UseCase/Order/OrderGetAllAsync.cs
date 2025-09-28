using Applications.Interface.DeliveryType;
using Applications.Interface.Order;
using Applications.Interface.Order.IOrder;
using Applications.Interface.Status;
using Applications.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.Order
{
    public class OrderGetAllAsync :IOrderGetAllAsync
    {
        private readonly IOrderQuery _query;
        private readonly IStatusQuery _statusQuery;
        private readonly IDeliveryTypeQuery _deliveryTypeQuery;

        public OrderGetAllAsync(IOrderQuery query, IStatusQuery statusQuery, IDeliveryTypeQuery deliveryTypeQuery)
        {
            _query = query;
            _statusQuery = statusQuery;
            _deliveryTypeQuery = deliveryTypeQuery;
        }

        public async Task<IEnumerable<OrderDetailsResponse?>> GetOrderWithFilter(int? statusId, DateTime? from, DateTime? to)
        {
            var orders = await _query.GetOrderWithFilter(statusId, from, to);

            if (orders == null || !orders.Any())
            {
                return Enumerable.Empty<OrderDetailsResponse?>();
            }

            var orderResponses = orders.Select(order =>
            new OrderDetailsResponse
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
            });
            return orderResponses;
        }
    }
}
