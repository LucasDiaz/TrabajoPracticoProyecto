using Applications.Interface.Category;
using Applications.Interface.Dish;
using Applications.Interface.Dish.IDishService;
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

        public DishDelete(IDishCommand command, IDishQuery query)
        {
            _command = command;
            _query = query;
        }

        public async Task<DishResponse> DeleteDishAsync(Guid id)
        {
            var dish = await _query.GetDishById(id);
            
            dish.Available = false; // Set the dish as inactive before deletion
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
