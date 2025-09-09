using Applications.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interface.Dish
{
    public interface IDishCommand
    {
        Task CreateDish(Domain.Entities.Dish model);
        Task UpdateDish( Domain.Entities.Dish model);
        Task DeleteDish(Guid id);
    }
}
