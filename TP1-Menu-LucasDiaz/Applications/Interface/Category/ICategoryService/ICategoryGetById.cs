using Applications.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Interface.Category.ICategoryService
{
    public interface ICategoryGetById
    {
        Task<CategoryResponse?> GetCategoryById(int id);
    }
}
