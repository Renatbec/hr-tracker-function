using Azure;
using Azure.Data.Tables;
using System;

namespace SailboatTracker.Models
{
	public class BoatEntity : ITableEntity
	{
		// Obligatoriske metadata for Azure Table Storage
		public string PartitionKey { get; set; } = "HallbergRassy";
		public string RowKey { get; set; } = Guid.NewGuid().ToString(); // Genereres automatisk

		// Egendefinerte felter for båtinformasjon
		public string Model { get; set; }
		public string BoatName { get; set; }
		public int LengthFeet { get; set; }
		public int YearBuilt { get; set; }
		public string LocationSpotted { get; set; }

		// Felt for å registrere når båten ble observert
		// Vi bruker DateTime.SpecifyKind for å sikre at verdien tolkes som UTC
		// Dette er viktig fordi Azure Table Storage forventer tidspunkter i UTC-format
		private DateTime _datespotted;
		public DateTime DateSpotted
		{
			get => _datespotted;
			set => _datespotted = DateTime.SpecifyKind(value, DateTimeKind.Utc);
		}

		// Systemegenskaper som settes automatisk av Azure Table Storage
		public DateTimeOffset? Timestamp { get; set; } // Siste endringstidspunkt
		public ETag ETag { get; set; } // Versjonsmarkør for concurrency
	}
}
