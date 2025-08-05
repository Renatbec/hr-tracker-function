using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SailboatTracker.Models
{
	public class BoatRequest
	{
		[JsonPropertyName("model")]
		public string Model { get; set; }
		[JsonPropertyName("boatName")]
		public string BoatName { get; set; }
		[JsonPropertyName("lengthFeet")]
		public int LengthFeet { get; set; }
		[JsonPropertyName("yearBuilt")]
		public int YearBuilt { get; set; }
		[JsonPropertyName("locationSpotted")]
		public string LocationSpotted { get; set; }
		[JsonPropertyName("dateSpotted")]
		public DateTime DateSpotted { get; set; }
	}

}
