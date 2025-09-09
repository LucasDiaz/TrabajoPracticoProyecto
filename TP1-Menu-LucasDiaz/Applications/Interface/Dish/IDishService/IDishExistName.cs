using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interface.Dish.IDishService
{
    public interface IDishExistName
    {
        Task<bool> DishExistsName(string name);
    }
}
