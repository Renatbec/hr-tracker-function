using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SailboatTracker.Models;
using System.Text.Json;

namespace SailboatTracker.Functions
{
	public class AddBoatFunction
	{
		private readonly ILogger<AddBoatFunction> _logger;

		public AddBoatFunction(ILogger<AddBoatFunction> logger)
		{
			_logger = logger;
		}

		// Azure Function som trigges via HTTP POST til /api/boats
		[Function("AddBoat")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = "boats")] HttpRequest req)
		{
			// Les inn hele forespørselens body som tekst
			var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

			// Deserialiser JSON til BoatRequest-objekt
			var boatRequest = JsonSerializer.Deserialize<BoatRequest>(requestBody);

			// Opprett en ny BoatEntity basert på innkommende data
			var boatEntity = new BoatEntity
			{
				Model = boatRequest.Model,
				BoatName = boatRequest.BoatName,
				LengthFeet = boatRequest.LengthFeet,
				YearBuilt = boatRequest.YearBuilt,
				LocationSpotted = boatRequest.LocationSpotted,
				DateSpotted = boatRequest.DateSpotted,

				// RowKey genereres automatisk for unik identifikasjon
				RowKey = Guid.NewGuid().ToString(),

				// PartitionKey brukes til å gruppere entiteter – kan endres etter behov
				PartitionKey = "HallbergRassy"

			};

			// Opprett TableClient for å kommunisere med Azure Table Storage
			var tableClient = new TableClient(
				Environment.GetEnvironmentVariable("AzureWebJobsStorage"),
				"Boats");

			// Lagre entiteten i tabellen
			await tableClient.AddEntityAsync(boatEntity);

			// Hent entiteten på nytt for å få med systemegenskaper som Timestamp og ETag
			var response = await tableClient.GetEntityAsync<BoatEntity>(
				boatEntity.PartitionKey,
				boatEntity.RowKey);

			// Returner den lagrede entiteten som JSON-respons
			return new OkObjectResult(response.Value);
		}
	}
}
