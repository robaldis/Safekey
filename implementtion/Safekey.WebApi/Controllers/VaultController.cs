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
        private readonly VaultCatalogue _vaultCatalogue;

        public VaultController(ILogger<VaultController> logger, VaultCatalogue vaultCatalogue)
        {
            _logger = logger;
            _vaultCatalogue = vaultCatalogue;
        }

        [HttpPost]
        [Route("{key}", Name = "CreateSecret")]
        [SwaggerOperation(OperationId = "CreateSecret")]
        [SwaggerResponse(StatusCodes.Status201Created)]
        public ActionResult CreateSecret(
                [FromRoute] string key)
        {
            Console.WriteLine($"key: {key}");
            var secret = _vaultCatalogue.CreateSecret(key);
            Console.WriteLine(secret);
            var a = _vaultCatalogue.GetSecret(secret.Item1, secret.Item2);
            Console.WriteLine(a);

            return new CreatedResult();
        }

        //[HttpGet]
        //[Route("{fileName}", Name = "GetFile")]
        //[SwaggerOperation(OperationId = "GetFile")]
        //[SwaggerResponse(StatusCodes.Status200OK)]
        //public async Task<ActionResult<FileResponse>> GetFile(
        //        [FromRoute] string fileName)
        //{
        //    return await HandleRequest(() => _fileApi.ReadFile(fileName));
        //}

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

