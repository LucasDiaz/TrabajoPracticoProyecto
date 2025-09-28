using Applications.Interface.Status.IStatusService;
using Applications.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TP1_Menu_LucasDiaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusGetAll _getAllStatus;
        public StatusController(IStatusGetAll getAllStatus)
        {
            _getAllStatus = getAllStatus;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StatusResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllStatuses()
        {
            var statuses = await _getAllStatus.GetAllStatuses();
            return Ok(statuses);
        }
    }
}
