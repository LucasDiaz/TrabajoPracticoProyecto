using Applications.Enum;
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
    public class UpdateItemFromOrder : IUpdateItemFromOrder
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
            var order = await _query.GetOrderById(orderId);

            if (order == null)
            {
                throw new NullException($"Orden con ID {orderId} no encontrada");
            }

            if (order.OverallStatus.Id != 1)
            {
                string currentStatusName = order.OverallStatus?.Name ?? "Desconocido";
                throw new RequeridoException($"La orden está en estado '{currentStatusName}' y no se puede modificar.");
            }

            // 3. Validar la lista de ítems de la solicitud
            if (listItems.items == null || !listItems.items.Any())
            {
                throw new RequeridoException("La orden debe contener al menos un plato.");
            }

            var dishIds = listItems.items.Select(i => i.Id).ToList();
            var dishesFromDb = await _DishQuery.GetDishesByIds(dishIds);
            var dishesDictionary = dishesFromDb.ToDictionary(d => d.DishId);

            // VALIDACIÓN INTEGRAL DE LOS RESULTADOS OBTENIDOS
            if (dishesFromDb.Count != dishIds.Count)
            {
                throw new RequeridoException("Uno o más platos especificados no existen.");
            }
            if (dishesFromDb.Any(d => !d.Available))
            {
                throw new RequeridoException("Uno o más platos especificados no están disponibles.");
            }

                // Iteramos sobre los ítems solicitados para validar disponibilidad y cantidad
            foreach (var item in listItems.items)
            {
                // Validar Disponibilidad y Cantidad usando el diccionario cargado
                if (!dishesDictionary.TryGetValue(item.Id, out var dish) || !dish.Available)
                {
                    throw new RequeridoException($"El plato con ID {item.Id} no existe o no está disponible.");
                }

                if (item.quantity <= 0)
                {
                    throw new RequeridoException("La cantidad debe ser mayor a 0");
                }
            }
            var itemsToRemove = new List<OrderItem>();

            foreach (var itemRequest in listItems.items)
            {
                var existingItem = order.OrderItems.FirstOrDefault(oi => oi.DishId == itemRequest.Id);

                if (itemRequest.quantity > 0)
                {
                    // --- LÓGICA DE AÑADIR O ACTUALIZAR ---
                    var dish = dishesDictionary[itemRequest.Id]; // Plato ya validado

                    if (existingItem != null)
                    {
                        // Actualizar ítem existente
                        existingItem.Quantity = itemRequest.quantity;
                        existingItem.Notes = itemRequest.notes;
                        // EF Core rastrea esto como 'Modified'
                    }
                    else
                    {
                        // Añadir ítem nuevo
                        var newItem = new OrderItem
                        {
                            // OrderId se setea por EF por navegación
                            DishId = itemRequest.Id,
                            Quantity = itemRequest.quantity,
                            Notes = itemRequest.notes,
                            StatusId = 1, // Pendiente
                        };
                        order.OrderItems.Add(newItem); // EF rastrea esto como 'Added'
                    }
                }
                else // itemRequest.quantity == 0
                {
                    // --- LÓGICA DE BORRADO ---
                    if (existingItem != null)
                    {
                        // 1. Añadimos a la lista de ítems a borrar
                        itemsToRemove.Add(existingItem);
                    }
                    // Si no existía y la cantidad es 0, no hacemos nada.
                }
            }

            // --- 4. EJECUTAR BORRADOS ---
            if (itemsToRemove.Any())
            {
                // 1. Borrar de la Base de Datos (marca como 'Deleted' en el contexto)
                await _OrderItemCommand.RemoveOrderItem(itemsToRemove);

                // 2. (¡EL PASO CLAVE!) Borrar de la lista en memoria
                // Esto asegura que Calculate() sea correcto y que
                // _context.Update(order) no intente re-agregar los ítems.
                foreach (var item in itemsToRemove)
                {
                    order.OrderItems.Remove(item);
                }
            }
            if (!order.OrderItems.Any())
            {
                // ¡Si la orden quedó vacía (todos los ítems en 0),
                // la marcamos como Cancelada!
                // (Asumo que 5 es 'Canceled' según tu OpenApi)
                order.StatusId = (int)OrderStatus.Closed;
            }


            // 5. Crear e Insertar los nuevos ítems
            var newOrderItems = listItems.items.Select(item => new OrderItem
            {
                OrderId = orderId,
                DishId = item.Id,
                Quantity = item.quantity,
                Notes = item.notes,
                StatusId = 1
            }).ToList();

            await _OrderItemCommand.InsertOrderItemRange(newOrderItems);

            // 6. Recalcular y Actualizar la orden
            // Corregido: Llamamos al método Calculate (ahora síncrono en la práctica si quitamos el await)
            order.Price = Calculate(newOrderItems, dishesFromDb); // Ya no necesita 'await'
            order.UpdateDate = DateTime.Now;
            await _command.UpdateOrder(order); // Requiere SaveChangesAsync en el Command

            // 7. Retornar la respuesta (Se mantiene igual)
            return new OrderUpdateReponse
            {
                orderNumber = (int)order.OrderId,
                totalAmount = (double)order.Price,
                UpdateAt = order.UpdateDate
            };

        }
        private decimal Calculate(List<OrderItem> newOrderItems, List<Dish> dishes)
        {
            // ... La lógica de cálculo se mantiene igual y es ahora síncrona ...
            decimal total = 0;
            var dishObt = dishes.ToDictionary(d => d.DishId);

            foreach (var item in newOrderItems)
            {
                if (dishObt.TryGetValue(item.DishId, out var dish))
                {
                    total += dish.Price * item.Quantity;
                }
            }
            return total;
        }
        
    }

}