using Applications.Interface.Category;
using Applications.Interface.Category.ICategoryService;
using Applications.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.UseCase.CategoryService
{
    public class CategoryGetAll : ICategoryGetAll
    {
        private readonly ICategoryQuery _categoryQuery;

        public CategoryGetAll(ICategoryQuery categoryQuery)
        {
            _categoryQuery = categoryQuery;
        }

        public async Task<List<CategoryResponse>> CategoriesGetAll()
        {
            var categories = await _categoryQuery.GetAllCategories();

            return categories.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
        }
    }
}
