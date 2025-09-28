using Applications.Interface.Dish;
using Applications.Interface.Dish.IDishService;
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
        public DishGetById(IDishQuery dishRepository)
        {
            _dishRepository = dishRepository;
        }
        public async Task<DishResponse?> GetDishById(Guid id)
        {
            var dish = await _dishRepository.GetDishById(id);
            

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
