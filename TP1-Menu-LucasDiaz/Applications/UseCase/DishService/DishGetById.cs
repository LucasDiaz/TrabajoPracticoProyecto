using Applications.Exceptions;
using Applications.Interface.Category;
using Applications.Interface.Dish;
using Applications.Interface.Dish.IDishService;
using Applications.Models.Request;
using Applications.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.DishService
{
    public class DishGetById : IDishGetById
    {
        private readonly IDishQuery _dishRepository;
        private readonly ICategoryQuery _categoryQuery;

        public DishGetById(IDishQuery dishRepository, ICategoryQuery categoryQuery)
        {
            _dishRepository = dishRepository;
            _categoryQuery = categoryQuery;
        }

        public async Task<DishResponse?> GetDishById(Guid id)
        {
            //if (string.IsNullOrWhiteSpace(id)) { 
            
            //}
            var dish = await _dishRepository.GetDishById(id);
            if (dish == null) 
            {
                //404
                throw new RequeridoException($"El plato con ID {id}  no existe.");
            }
            if (dish.Category.Id != 0)
            {
                var categoryExists = await _categoryQuery.GetExistCategory(dish.Category.Id);
                if (!categoryExists)
                {
                    //400
                    throw new RequeridoException("Required Category data.");
                }
            }


            return new DishResponse
            {
                Id = dish.DishId,
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                Category = new GenericResponse { Id = dish.Category.Id, Name = dish.Category?.Name },
                isActive = dish.Available,
                Image = dish.ImageUrl,
                createdAt = dish.CreateDate,
                updateAt = dish.UpdateDate
            };
        }
    }
}
