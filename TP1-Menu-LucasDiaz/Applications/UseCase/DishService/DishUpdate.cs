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
            var existingDish = await _query.GetDishById(id);
            var CategoryExits = await _categoryQuery.GetCategoryById(DishUpdateRequest.Category);
            


            existingDish.Name = DishUpdateRequest.Name;
            existingDish.Description = DishUpdateRequest.Description;
            existingDish.Price = DishUpdateRequest.Price;
            existingDish.CategoryId = DishUpdateRequest.Category;
            existingDish.Category = CategoryExits;
            existingDish.Available = DishUpdateRequest.IsActive;
            existingDish.ImageUrl = DishUpdateRequest.Image;
            existingDish.UpdateDate = DateTime.UtcNow;

            await _command.UpdateDish(existingDish);

            return new DishResponse
            {
                Id = existingDish.DishId,
                Name = existingDish.Name,
                Description = existingDish.Description,
                Price = existingDish.Price,
                Category = new GenericResponse { Id = existingDish.Category.Id, Name = existingDish.Category.Name },
                isActive = existingDish.Available,
                Image = existingDish.ImageUrl,
                createdAt = existingDish.CreateDate,
                updateAt = existingDish.UpdateDate
            };
        }
    }
}
