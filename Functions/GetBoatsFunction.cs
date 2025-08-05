using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SailboatTracker.Models;

namespace SailboatTracker.Functions
{
    public class GetBoatsFunction
    {
        private readonly ILogger<GetBoatsFunction> _logger;

        public GetBoatsFunction(ILogger<GetBoatsFunction> logger)
        {
            _logger = logger;
        }

		[Function("GetBoats")]
		public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "boats")] HttpRequest req, ILogger log)
		{
			_logger.LogInformation("C# HTTP trigger function processed a request.");

			var tableClient = new TableClient(
				Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
				"Boats");

			var boats = tableClient.Query<BoatEntity>(b => b.PartitionKey == "HallbergRassy").ToList();

			return new OkObjectResult(boats);
		}

	}
}
