using Applications.Exceptions;
using Applications.Interface.Order.IOrder;
using Applications.Models.Request;
using Applications.Models.Response;
using Applications.UseCase.Order;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static System.Net.Mime.MediaTypeNames;

namespace TP1_Menu_LucasDiaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderCreate _OrderCreate;
        private readonly IOrderGetById _OrderGetById;
        private readonly IOrderGetAllAsync _OrderGetAllAsync;
        private readonly IUpdateItemFromOrder _updateItemFromOrder;
        private readonly IUpdateOrderItemStatus _updateOrderItemStatus;

        public OrderController(IOrderCreate orderCreate, IOrderGetById orderGetById, IOrderGetAllAsync orderGetAllAsync, IUpdateItemFromOrder updateItemFromOrder, IUpdateOrderItemStatus updateOrderItemStatus)
        {
            _OrderCreate = orderCreate;
            _OrderGetById = orderGetById;
            _OrderGetAllAsync = orderGetAllAsync;
            _updateItemFromOrder = updateItemFromOrder;
            _updateOrderItemStatus = updateOrderItemStatus;
        }

        // POST
        /// <summary>
        /// Crear nueva orden.
        /// </summary>
        /// <remarks>
        /// Crea un nueva orden en el menú del restaurante.
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(OrderCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderRequest)
        {

            try
            {
                var response = await _OrderCreate.CreateOrder(orderRequest);
                return StatusCode(201, response); // HTTP 201 Created
            }
            catch (RequeridoException ex)
            {
               
                return BadRequest(new { message = ex.Message }); // HTTP 400 Bad Request
            }
        }

        // GET with filters
        /// <summary>
        /// Buscar órdenes.
        /// </summary>
        /// <remarks>
        /// Obtiene una lista de órdenes con filtros opcionales.
        /// </remarks>
        //("search")
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderDetailsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrders([FromQuery] int? statusId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            //sacar try

            try
            {
                var orders = await _OrderGetAllAsync.GetOrderWithFilter(statusId, from, to);
                return Ok(orders); // HTTP 200 OK con la lista de órdenes
            }
            catch (NullException ex)
            {
                // Capturamos la excepción lanzada en el servicio.
                // Un 404 es apropiado cuando no se encuentran recursos.
                return NotFound(new { message = ex.Message });
            }
           
        }

        //GET by ID
        /// <summary>
        /// Obtiene una order por su ID.
        /// </summary>
        /// <remarks>
        /// Busca un order específico en el menú usando su identificador único.
        /// </remarks>
        [HttpGet("{id}")]
        [SwaggerOperation(
        Summary = "Buscar orders por ID",
        Description = "Buscar orders por ID."
        )]
        [ProducesResponseType(typeof(OrderDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(long id)
        {
            try
            {
                var order = await _OrderGetById.GetOrderById(id);

                return Ok(order); // HTTP 200 OK
            }
            catch (NullException ex)
            {
                // Capturamos la InvalidOperationException lanzada por el servicio
                // y la mapeamos al código HTTP 404 Not Found.
                return NotFound(new { message = ex.Message });
            }
        }
        // PUT to update order items
        [HttpPut("{orderId}")]
        [ProducesResponseType(typeof(OrderUpdateReponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrderItems(long orderId, [FromBody] OrderUpdateRequest request)
        {
            try { 
            var response = await _updateItemFromOrder.UpdateItemQuantity(orderId, request);
            return Ok(response); }
            catch (NullException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (RequeridoException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
        // PATCH: api/v1/order/1001/item/1
        [HttpPatch("{orderId}/item/{itemId}")]
        [ProducesResponseType(typeof(OrderUpdateReponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        //aplicar
        public async Task<IActionResult> UpdateOrderItemStatus(long orderId, int itemId, [FromBody] OrderItemUpdateRequest request)
        {
            try { 
            var response = await _updateOrderItemStatus.UpdateItemStatus(orderId, itemId, request);
            return Ok(response);
            }
            catch (NullException ex)
            {
                return NotFound(new { message = ex.Message });
            }
           
            catch (RequeridoException ex)
            {
                return BadRequest(new { message = ex.Message });
            }


        }
    }
}
