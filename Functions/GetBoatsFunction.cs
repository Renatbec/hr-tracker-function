using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SailboatTracker.Functions
{
    public class GetBoatsFunction
    {
        private readonly ILogger<GetBoatsFunction> _logger;

        public GetBoatsFunction(ILogger<GetBoatsFunction> logger)
        {
            _logger = logger;
        }

        [Function("GetBoatsFunction")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
