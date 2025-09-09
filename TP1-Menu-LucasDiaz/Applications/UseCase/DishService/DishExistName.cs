using Applications.Interface.Dish;
using Applications.Interface.Dish.IDishService;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.DishService
{
    public class DishExistName: IDishExistName
    {
        private readonly IDishQuery _query;

        public DishExistName(IDishQuery query)
        {
            _query = query;
        }

        public async Task<bool> DishExistsName(string name)
        {
           
            return await _query.GetDishByName(name);
        }
    }
}
