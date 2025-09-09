using Applications.Enum;    
using Applications.Interface.Category.ICategoryService;
using Applications.Interface.Dish.IDishService;
using Applications.Models.Request;
using Applications.Models.Response;
using Applications.UseCase.CategoryService;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TP1_Menu_LucasDiaz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishCreate _dishCreate;
        private readonly IDishGetAllAsync _dishGetAllAsync;
        private readonly IDishUpdate _dishUpdate;
        private readonly ICategoryExist _categoryExist;
        private readonly IDishExistName _dishExistName;
        private readonly IDishExistId _dishExistId;

        public DishController(IDishCreate dishCreate, IDishGetAllAsync dishGetAllAsync, IDishUpdate dishUpdate, ICategoryExist categoryExist, IDishExistName dishExistName, IDishExistId dishExistId)
        {
            _dishCreate = dishCreate;
            _dishGetAllAsync = dishGetAllAsync;
            _dishUpdate = dishUpdate;
            _categoryExist = categoryExist;
            _dishExistName = dishExistName;
            _dishExistId = dishExistId;
        }





        // POST
        /// <summary>
        /// Crear nuevo plato.
        /// </summary>
        /// <remarks>
        /// Crea un nuevo plato en el menú del restaurante.
        /// </remarks>
        [HttpPost]
        [SwaggerOperation(
        Summary = "Crear nuevo plato",
        Description = "Crea un nuevo plato en el menú del restaurante."
        )]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDish([FromBody] DishRequest dishRequest)
        {
            if (dishRequest == null)
            {
                return BadRequest(new ApiError("Required dish data."));
            }
            if (string.IsNullOrWhiteSpace(dishRequest.Name))
            {
                return BadRequest(new ApiError("Name is required."));
            }
            if (dishRequest.Category != 0)
            {
                var categoryExists = await _categoryExist.CategoryExists(dishRequest.Category);
                if (!categoryExists)
                {
                    return BadRequest(new ApiError("Required CategoryId."));
                }
            }
            if (dishRequest.Price <= 0)
            {
                return BadRequest(new ApiError("Price must be greater than zero."));
            }

            var createdDish = await _dishCreate.CreateDish(dishRequest);
            
            if (createdDish == null)
            {
                return Conflict(new ApiError("A dish with this name already exists."));
            }
            return CreatedAtAction(nameof(Search), new { id = createdDish.Id }, createdDish);

        }
        // GETs
        // GET with filters
        /// <summary>
        /// Busca platos.
        /// </summary>
        /// <remarks>
        /// Obtiene una lista de platos del menú con opciones de filtrado y ordenamiento.
        /// </remarks>
        //("search")
        [HttpGet]
        [SwaggerOperation(
        Summary = "Buscar platos",
        Description = "Obtiene una lista de platos del menú con opciones de filtrado y ordenamiento."
        )]
        [ProducesResponseType(typeof(IEnumerable<DishResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Search(
            [FromQuery] string? name,
            [FromQuery] int? categoryId,
            [FromQuery] OrderPrice? orderPrice = OrderPrice.ASC,
            [FromQuery] bool onlyActive = true)
        {

            
            var list = await _dishGetAllAsync.SearchAsync(name, categoryId, orderPrice);

            if (categoryId != 0)
            {
                var categoryExists = await _categoryExist.CategoryExists(categoryId.Value);
                if (!categoryExists)
                {
                    return BadRequest(new ApiError("Required CategoryId."));
                }
            }
            if (list == null || !list.Any())
            {
                return NotFound(new ApiError("No dishes found matching the criteria."));
            }
            if (onlyActive)
            {
                list = list.Where(d => d.isActive);
            }
            if (!onlyActive)
            {
                list = list.Where(d => d.isActive == false);
            }

            return Ok(list);

        }
       
        /// <summary>
        /// Obtiene un plato por su ID.
        /// </summary>
        /// <remarks>
        /// Busca un plato específico en el menú usando su identificador único.
        /// </remarks>



        // PUT
        /// <summary>
        /// Actualizar plato existente.
        /// </summary>
        /// <remarks>
        /// Actualiza todos los campos de un plato existente en el menú.
        /// </remarks>

        [HttpPut("{id}")]
        [SwaggerOperation(
        Summary = "Actualizar plato existente",
        Description = "Actualiza todos los campos de un plato existente en el menú."
        )]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateDish(Guid id, [FromBody] DishUpdateRequest dishRequest)
        {
            if (dishRequest == null)
            {
                return BadRequest(new ApiError("Required dish data."));
            }
            if (string.IsNullOrWhiteSpace(dishRequest.Name))
            {
                return BadRequest(new ApiError("Name is required."));
            }
            var existingDish = await _dishExistId.DishExistsID(id);
            if (!existingDish)
            {
                return NotFound(new ApiError($"Dish with ID {id} not found."));
            }
            if (await _dishExistName.DishExistsName(dishRequest.Name))
            {
                return Conflict(new ApiError($"Dish {dishRequest.Name} already exists."));
            }

            if (dishRequest.Category == 0)
            {
                return BadRequest(new ApiError("Category is required."));
            }
            if (dishRequest.Category != 0)
            {
                var categoryExists = await _categoryExist.CategoryExists(dishRequest.Category);
                if (!categoryExists)
                {
                    return BadRequest(new ApiError("Required CategoryId."));
                }
            }

            if (dishRequest.Price <= 0)
            {
                return BadRequest(new ApiError("Price must be greater than zero."));
            }

            var result = await _dishUpdate.UpdateDish(id, dishRequest);
          

            return Ok(result);
        }

        // DELETE
    }
}
