
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Safekey.Webapi.Controllers
{

    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("", Name = "GetHealth")]
        [SwaggerOperation(OperationId = "GetHealth")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult HealthCheck()
        {
            return Ok();
        }
    }
}

