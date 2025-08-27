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
		private readonly TableClient _tableClient;

		public GetBoatsFunction(ILogger<GetBoatsFunction> logger, TableClient tableClient)
        {
            _logger = logger;
			_tableClient = tableClient;
		}

		[Function("GetBoats")]
		public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "boats")] HttpRequest req, ILogger log)
		{
			try
			{
				_logger.LogInformation("C# HTTP trigger function processed a request.");

				//var boats = _tableClient.Query<BoatEntity>(b => b.PartitionKey == "HallbergRassy").ToList();

				var boats = new List<BoatEntity>();

				await foreach (var boat in _tableClient
					.QueryAsync<BoatEntity>(b => b.PartitionKey == "HallbergRassy"))
				{
					boats.Add(boat);
				}

				return new OkObjectResult(boats);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error accessing the table.");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
				
		}

	}
}
