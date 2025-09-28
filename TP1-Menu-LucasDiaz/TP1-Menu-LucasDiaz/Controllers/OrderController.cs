using Applications.Interface.Order.IOrder;
using Applications.Models.Request;
using Applications.Models.Response;
using Applications.UseCase.Order;
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
                var result = await _OrderCreate.CreateOrder(orderRequest);

                return CreatedAtAction(nameof(CreateOrder), new { id = result.orderNumber }, result);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new ApiError("An error occurred while processing the request."));
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
                var result = await _OrderGetAllAsync.GetOrderWithFilter(statusId, from, to);
                if (result == null || !result.Any())
                {
                    return NotFound(new ApiError("No orders found with the specified filters."));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new ApiError("An error occurred while processing the request."));
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
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(long id)
        {
            var order = await _OrderGetById.GetOrderById(id);
            //if (dish == null)
            //{
            //    throw new NotFoundException($"Order with ID {id} not found.");
            //}
            return Ok(order);
        }
        // PUT to update order items
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrderItems(long orderId, [FromBody] OrderUpdateRequest request)
        {
            var response = await _updateItemFromOrder.UpdateItemQuantity(orderId, request);
            return Ok(response);
        }
        // PATCH: api/v1/order/1001/item/1
        [HttpPatch("{orderId}/item/{itemId}")]
        //aplicar
        public async Task<IActionResult> UpdateOrderItemStatus(long orderId, int itemId, [FromBody] OrderItemUpdateRequest request)
        {
            var response = await _updateOrderItemStatus.UpdateItemStatus(orderId, itemId, request);
            return Ok(response);
        }
    }
}
