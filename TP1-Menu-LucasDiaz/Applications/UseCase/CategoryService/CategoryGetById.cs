using Applications.Interface.Category;
using Applications.Interface.Category.ICategoryService;
using Applications.Interface.Dish;
using Applications.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.CategoryService
{
    public class CategoryGetById: ICategoryGetById
    {
        private readonly ICategoryQuery categoryQuery;

        public CategoryGetById(ICategoryQuery categoryQuery)
        {
            this.categoryQuery = categoryQuery;
        }

        public Task<CategoryResponse?> GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
