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
		private readonly TableClient _tableClient;

		public DeleteAllBoatsFunction(ILogger<DeleteAllBoatsFunction> logger, TableClient tableClient)
		{
			_logger = logger;
			_tableClient = tableClient;
		}

		[Function("DeleteAllBoats")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "delete", Route = "boats")] HttpRequest req)
		{
			try
			{
				int deletedCount = 0;

				// Query all entities in the table
				await foreach (var entity in _tableClient.QueryAsync<TableEntity>())
				{
					await _tableClient.DeleteEntityAsync(entity.PartitionKey, entity.RowKey);
					deletedCount++;
				}

				_logger.LogInformation($"Deleted {deletedCount} boats from the table.");

				return new OkObjectResult($"{deletedCount} boats deleted from the table.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error accessing the table.");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
