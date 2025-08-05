using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SailboatTracker.Functions
{
	public class DeleteAllBoatsFunction
	{
		private readonly ILogger<DeleteAllBoatsFunction> _logger;

		public DeleteAllBoatsFunction(ILogger<DeleteAllBoatsFunction> logger)
		{
			_logger = logger;
		}

		[Function("DeleteAllBoats")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "delete", Route = "boats")] HttpRequest req)
		{
			var tableClient = new TableClient(
				Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
				"Boats");

			int deletedCount = 0;

			// Query all entities in the table
			await foreach (var entity in tableClient.QueryAsync<TableEntity>())
			{
				await tableClient.DeleteEntityAsync(entity.PartitionKey, entity.RowKey);
				deletedCount++;
			}

			_logger.LogInformation($"Deleted {deletedCount} boats from the table.");

			return new OkObjectResult($"{deletedCount} boats deleted from the table.");
		}
	}
}
