using Applications.Interface.DeliveryType.IDeliveryTypeService;
using Applications.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TP1_Menu_LucasDiaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryTypeController : ControllerBase
    {
        private readonly IDeliveryTypeGetAll _getallDeliverys;
        public DeliveryTypeController(IDeliveryTypeGetAll getallDeliverys)
        {
            _getallDeliverys = getallDeliverys;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeliveryTypeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllDeliveryTypes()
        {
            var deliveryTypes = await _getallDeliverys.GetAllDeliveryType();
            return Ok(deliveryTypes);
        }
    }
}
