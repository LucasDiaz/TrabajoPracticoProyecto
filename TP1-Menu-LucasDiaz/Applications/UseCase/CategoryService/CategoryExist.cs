using Applications.Interface.Category;
using Applications.Interface.Category.ICategoryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.CategoryService
{
    public class CategoryExist : ICategoryExist
    {
        private readonly ICategoryQuery _categoryQuery;
        public CategoryExist(ICategoryQuery categoryQuery)
        {
            _categoryQuery = categoryQuery;
        }

        public async Task<bool> CategoryExists(int categoryId)
        {
            var category = await _categoryQuery.GetCategoryById(categoryId);

         
            return category != null;

        }
    }
}
