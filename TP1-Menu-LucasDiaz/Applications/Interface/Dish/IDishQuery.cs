using Applications.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interface.Dish
{
    public interface IDishQuery
    {
        Task<IEnumerable<Domain.Entities.Dish>> GetAllDishes();
        Task<bool> GetDishByName(string name);
        Task<Domain.Entities.Dish?> GetDishById(Guid id);
        Task<IEnumerable<Domain.Entities.Dish?>> GetAllAsync(string? name = null, int? categoryId = null, OrderPrice? priceOrder = OrderPrice.ASC);

        Task<List<Domain.Entities.Dish>> GetDishesByIds(List<Guid> ids);
        Task<bool> DishExistsById(Guid id);
        Task<bool> DishExists(string name, Guid? id);
    }
}
