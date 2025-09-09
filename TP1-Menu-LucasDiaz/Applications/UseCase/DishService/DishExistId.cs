using Applications.Interface.Category;
using Applications.Interface.Dish;
using Applications.Interface.Dish.IDishService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.DishService
{
    public class DishExistId: IDishExistId
    {
        private readonly IDishQuery _query;

        public DishExistId(IDishQuery query)
        {
            _query = query;
        }

        public async Task<bool> DishExistsID(Guid categoryId)
        {
            var dish = await _query.GetDishById(categoryId);


            return dish != null;
        }
    }
}
