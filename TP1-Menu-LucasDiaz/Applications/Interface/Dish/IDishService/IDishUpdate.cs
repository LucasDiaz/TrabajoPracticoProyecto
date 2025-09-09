using Applications.Models.Request;
using Applications.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interface.Dish.IDishService
{
    public interface IDishUpdate
    {
        Task<DishResponse> UpdateDish(Guid id, DishUpdateRequest DishUpdateRequest);
    }
}
