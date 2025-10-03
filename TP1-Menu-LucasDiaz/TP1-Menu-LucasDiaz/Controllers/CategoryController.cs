using Applications.Interface.Category.ICategoryService;
using Applications.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TP1_Menu_LucasDiaz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryGetAll _CategoryGetAll;

        public CategoryController(ICategoryGetAll getAllCategoryAsyncUseCase)
        {
            _CategoryGetAll = getAllCategoryAsyncUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryTypeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _CategoryGetAll.CategoriesGetAll();
            return Ok(categories);
        }
    }
}
