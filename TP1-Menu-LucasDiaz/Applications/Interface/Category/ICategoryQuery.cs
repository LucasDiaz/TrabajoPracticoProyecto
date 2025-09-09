using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Applications.Interface.Category
{
    public interface ICategoryQuery
    {
        Task<List<Domain.Entities.Category>> GetAllCategories();
        Task<Domain.Entities.Category?> GetCategoryById(int id);
    }
}
