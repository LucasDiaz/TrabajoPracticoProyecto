using Applications.Enum;
using Applications.Interface.Category;
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
    public class DishGetAllAsync : IDishGetAllAsync
    {
      
        private readonly IDishQuery _query;

        public DishGetAllAsync(IDishQuery query)
        {
            _query = query;
        }

        public async Task<IEnumerable<DishResponse?>> SearchAsync(string? name, int? categoryId, OrderPrice? priceOrder = OrderPrice.ASC)
        {

            var list = await _query.GetAllAsync(name, categoryId, priceOrder);
            return list.Select(dishes => new DishResponse
            {
                Id = dishes.DishId,
                Name = dishes.Name,
                Description = dishes.Description,
                Price = dishes.Price,
                Category = new GenericResponse { Id = dishes.CategoryId, Name = dishes.Category?.Name },
                isActive = dishes.Available,
                Image = dishes.ImageUrl,
                createdAt = dishes.CreateDate,
                updateAt = dishes.UpdateDate
            }).ToList();
        }



    }
}
