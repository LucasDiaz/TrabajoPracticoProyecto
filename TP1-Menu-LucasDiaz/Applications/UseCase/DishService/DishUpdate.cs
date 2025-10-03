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

namespace Applications.UseCase.DishService
{
    public class DishUpdate: IDishUpdate
    {
        private readonly IDishCommand _command;
        private readonly IDishQuery _query;
        private readonly ICategoryQuery _categoryQuery;
        
        public DishUpdate(IDishCommand command, IDishQuery query, ICategoryQuery categoryQuery)
        {
            _command = command;
            _query = query;
            _categoryQuery = categoryQuery;
        }

        public async Task<DishResponse> UpdateDish(Guid id, DishUpdateRequest DishUpdateRequest)
        {
            var Dish = await _query.GetDishById(id);
            var CategoryExits = await _categoryQuery.GetCategoryById(DishUpdateRequest.Category);
            if (Dish == null)
            {
                //404
                throw new NullException("Required dish data.");
            }
            if (string.IsNullOrWhiteSpace(DishUpdateRequest.Name))
            {
                //400
                throw new RequeridoException("Name is required.");
            }
            if (DishUpdateRequest.Category != 0)
            {
                var categoryExists = await _categoryQuery.GetExistCategory(DishUpdateRequest.Category);
                if (!categoryExists)
                {
                    //400
                    throw new RequeridoException("Required Category data.");
                }
            }
            if (DishUpdateRequest.Price <= 0)
            {
                //400
                throw new RequeridoException("Price must be greater than zero.");

            }
           
            var existingDish = await _query.DishExists(DishUpdateRequest.Name,id);

            if (existingDish)
            {
                //409
                throw new ConflictException("A dish with this name already exists.");
            }
           


            Dish.Name = DishUpdateRequest.Name;
            Dish.Description = DishUpdateRequest.Description;
            Dish.Price = DishUpdateRequest.Price;
            Dish.CategoryId = DishUpdateRequest.Category;
            Dish.Category = CategoryExits;
            Dish.Available = DishUpdateRequest.IsActive;
            Dish.ImageUrl = DishUpdateRequest.Image;
            Dish.UpdateDate = DateTime.UtcNow;

            await _command.UpdateDish(Dish);

            return new DishResponse
            {
                Id = Dish.DishId,
                Name = Dish.Name,
                Description = Dish.Description,
                Price = Dish.Price,
                Category = new GenericResponse { Id = Dish.Category.Id, Name = Dish.Category.Name },
                isActive = Dish.Available,
                Image = Dish.ImageUrl,
                createdAt = Dish.CreateDate,
                updateAt = Dish.UpdateDate
            };
        }
    }
}
