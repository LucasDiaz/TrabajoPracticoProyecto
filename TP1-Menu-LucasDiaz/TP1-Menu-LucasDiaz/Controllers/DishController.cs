using Applications.Enum;
using Applications.Exceptions;
using Applications.Interface.Category.ICategoryService;
using Applications.Interface.Dish.IDishService;
using Applications.Models.Request;
using Applications.Models.Response;
using Applications.UseCase.CategoryService;
using Azure.Core;
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
        private readonly IDishGetById _dishGetById;
        private readonly IDishDelete _deleteDish;

        public DishController(IDishCreate dishCreate, IDishGetAllAsync dishGetAllAsync, IDishUpdate dishUpdate, ICategoryExist categoryExist, IDishExistName dishExistName, IDishExistId dishExistId, IDishGetById dishGetById, IDishDelete deleteDish)
        {
            _dishCreate = dishCreate;
            _dishGetAllAsync = dishGetAllAsync;
            _dishUpdate = dishUpdate;
            _categoryExist = categoryExist;
            _dishExistName = dishExistName;
            _dishExistId = dishExistId;
            _dishGetById = dishGetById;
            _deleteDish = deleteDish;
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
            try
            {
                var createdDish = await _dishCreate.CreateDish(dishRequest);
                return CreatedAtAction(nameof(GetDishById), new { id = createdDish.Id }, createdDish);
                
            }
            catch (RequeridoException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ConflictException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
          


          
        }

        //GET by ID
        /// <summary>
        /// Obtiene un plato por su ID.
        /// </summary>
        /// <remarks>
        /// Busca un plato específico en el menú usando su identificador único.
        /// </remarks>
        [HttpGet("{id}")]
        [SwaggerOperation(
        Summary = "Buscar platos por ID",
        Description = "Buscar platos por ID."
        )]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDishById(Guid id)
        {
            try {                 
                var dish = await _dishGetById.GetDishById(id);
                return Ok(dish);
            }
            catch (NullException ex)
            {
                return NotFound(new ApiError(ex.Message));
            }
         
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
            [FromQuery] int? category,
            [FromQuery] OrderPrice? sortByPrice = OrderPrice.ASC,
            [FromQuery] bool? onlyActive = null)
        {



            try {
                var list = await _dishGetAllAsync.SearchAsync(name, category, sortByPrice);

                if (onlyActive != null)
                {
                    if (onlyActive == true)
                    {
                        list = list.Where(d => d.isActive);
                    }
                    if (onlyActive == false)
                    {
                        list = list.Where(d => d.isActive == false);
                    }
                }
                return Ok(list);

            }

             catch (NullException ex)
             {
                // Capturamos la excepción lanzada en el servicio.
                // Un 404 es apropiado cuando no se encuentran recursos.
                return NotFound(new { message = ex.Message });
             }
        }
       

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
            try { 
                var result = await _dishUpdate.UpdateDish(id, dishRequest);
                return Ok(result);
            }
            catch (NullException ex)
            {
              
                return NotFound(new { message = ex.Message });
            }
            catch (RequeridoException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ConflictException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        // DELETE
        /// <summary>
        /// Eliminar plato
        /// </summary>
        /// <remarks>
        /// Elimina un plato del menú del restaurante.
        /// </remarks>
        /// 
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteDish(Guid id)
        {
            try {
                var result = await _deleteDish.DeleteDishAsync(id);
                return Ok(result);
            }
            catch (NullException ex) {
                return BadRequest(new { message = ex.Message });
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }

        }


    }
}
