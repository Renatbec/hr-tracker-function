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
		private readonly TableClient _tableClient;

		public AddBoatFunction(ILogger<AddBoatFunction> logger, TableClient tableClient)
		{
			_logger = logger;
			_tableClient = tableClient;
		}

		// Azure Function som trigges via HTTP POST til /api/boats
		[Function("AddBoat")]
		public async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = "boats")] HttpRequest req)
		{
			try { 
				_logger.LogInformation("Behandler forespørsel om å legge til en ny båt.");

				// Les inn hele forespørselens body som tekst
				var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

				// Deserialiser JSON til BoatRequest-objekt
				var boatRequest = JsonSerializer.Deserialize<BoatRequest>(requestBody);

				if (boatRequest == null)
				{
					_logger.LogWarning("Ugyldig eller tom forespørsel.");
					return new BadRequestObjectResult("Ugyldig data.");
				}

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

				// Lagre entiteten i tabellen
				await _tableClient.AddEntityAsync(boatEntity);

				// Hent entiteten på nytt for å få med systemegenskaper som Timestamp og ETag
				var response = await _tableClient.GetEntityAsync<BoatEntity>(
					boatEntity.PartitionKey,
					boatEntity.RowKey);

				// Returner den lagrede entiteten som JSON-respons
				return new OkObjectResult(response.Value);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error adding boat entity to Azure Table Storage");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}

	}
}
