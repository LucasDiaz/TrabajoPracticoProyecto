using Applications.Exceptions;
using Applications.Interface.Category;
using Applications.Interface.Dish;
using Applications.Interface.Dish.IDishService;
using Applications.Models.Request;
using Applications.Models.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Applications.UseCase.DishService
{
    public class DishCreate : IDishCreate
    {

        private readonly IDishCommand _command;
        private readonly IDishQuery _query;
        private readonly ICategoryQuery _categoryQuery;
        public DishCreate(IDishCommand command, IDishQuery query, ICategoryQuery categoryQuery)
        {
            _command = command;
            _query = query;
            _categoryQuery = categoryQuery;
        }



        public async Task<DishResponse?> CreateDish(DishRequest dishRequest)
        {

            if (dishRequest == null)
            {
                //400
                throw new RequeridoException("Required dish data.");
            }
            if (string.IsNullOrWhiteSpace(dishRequest.Name))
            {
                //400
                throw new RequeridoException("Name is required.");
            }
            if (dishRequest.Category != 0)
            {
                var categoryExists = await _categoryQuery.GetExistCategory(dishRequest.Category);
                if (!categoryExists)
                {
                    //400
                    throw new RequeridoException("Required Category data.");
                }
            }
            if (dishRequest.Price <= 0) {
                //400
                throw new RequeridoException("Price must be greater than zero.");

            }

            //validaciones
            var existingDish = await _query.GetDishByName(dishRequest.Name);

            if (existingDish)
            {
                //409
                throw new ConflictException("A dish with this name already exists.");
            }
            var category = await _categoryQuery.GetCategoryById(dishRequest.Category);
            var dish = new Dish
            {
                DishId = Guid.NewGuid(),
                Name = dishRequest.Name,
                Description = dishRequest.Description,
                Price = dishRequest.Price,
                Available = true,
                ImageUrl = dishRequest.Image,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CategoryId = dishRequest.Category
            };
          
            await _command.CreateDish(dish);
         
            return new DishResponse
            {
                Id = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse { Id = category.Id, Name = category.Name },
                isActive = dish.Available,
                Image = dish.ImageUrl,
                createdAt = dish.CreateDate,
                updateAt = dish.UpdateDate
            };
        }
    }
}
