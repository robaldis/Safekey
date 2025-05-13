using Microsoft.AspNetCore.Mvc;
using Safekey.Service.Vault;
using Swashbuckle.AspNetCore.Annotations;

namespace Safekey.Webapi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class VaultController : ControllerBase
    {
        private readonly ILogger<VaultController> _logger;
        private readonly IVaultCatalogue _vaultCatalogue;

        public VaultController(ILogger<VaultController> logger, IVaultCatalogue vaultCatalogue)
        {
            _logger = logger;
            _vaultCatalogue = vaultCatalogue;
        }

        [HttpPost]
        [Route("{key}", Name = "CreateSecret")]
        [SwaggerOperation(OperationId = "CreateSecret")]
        [SwaggerResponse(StatusCodes.Status201Created)]
        public ActionResult CreateSecret(
                [FromRoute] string key,
                [FromBody] string secret)
        {
            _vaultCatalogue.CreateSecret(key, secret);
            return new CreatedResult();
        }

        [HttpGet]
        [Route("{key}", Name = "GetSecret")]
        [SwaggerOperation(OperationId = "GetSecret")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> GetFile(
                [FromRoute] string key)
        {
            var secret = _vaultCatalogue.GetSecret(key);
            return new ActionResult<string>(secret);
        }

        //public async Task<ActionResult<TResult>> HandleRequest<TResult>(
        //        Func<Task<Either<IFailure, TResult>>> apiFunc,
        //        int expectedStatusCode = StatusCodes.Status200OK)
        //{
        //    _logger.LogInformation($"Running {apiFunc.GetType().FullName}");
        //    var result = await apiFunc();
        //    return result.Match<ActionResult<TResult>>(
        //            Right: x => ProcessSuccess(x, expectedStatusCode),
        //            Left: e => Problem(
        //                title: e.Message,
        //                detail: e.DetailedMessage,
        //                statusCode: (int)e.StatusCode,
        //                instance: null,
        //                type: null));
        //}
        //private ActionResult<TResult> ProcessSuccess<TResult>(TResult input, int expectedStatusCode)
        //{
        //    switch(expectedStatusCode)
        //    {
        //        case StatusCodes.Status201Created:
        //            return (ActionResult<TResult>) new CreatedResult("location", input);
        //        case StatusCodes.Status204NoContent:
        //            return (ActionResult<TResult>) new NoContentResult();
        //        default: 
        //            return (ActionResult<TResult>) input;
        //    }
        //}
    }
}

