using Applications.Exceptions;
using Applications.Interface.Category;
using Applications.Interface.Dish;
using Applications.Interface.Dish.IDishService;
using Applications.Interface.IOrderItem;
using Applications.Models.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.DishService
{
    public class DishDelete: IDishDelete
    {
        private readonly IDishCommand _command;
        private readonly IDishQuery _query;
        private readonly IOrderItemQuery _orderItemquery;

        public DishDelete(IDishCommand command, IDishQuery query)
        {
            _command = command;
            _query = query;
        }

        public async Task<DishResponse> DeleteDishAsync(Guid id)
        {
            var dish = await _query.GetDishById(id);
            if (dish == null|| !dish.Available)
            {
                //400
                throw new NullException($"El plato con ID {id} no existe o no está disponible.");
            }
            bool usedInOrders = await _orderItemquery.ExistsByDishId(id);
            if (usedInOrders)
            {
                //409
                throw new ConflictException($"Dish with ID {id} cannot be deleted because it is used in existing orders.");
            }

            dish.Available = false; 
            await _command.UpdateDish(dish);
            return new DishResponse
            {
                Id = id,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse { Id = dish.Category.Id, Name = dish.Category.Name },
                isActive = dish.Available,
                Image = dish.ImageUrl,
                createdAt = dish.CreateDate,
                updateAt = dish.UpdateDate

            };

        }
    }
}
