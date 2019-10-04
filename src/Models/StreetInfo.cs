using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class StreetInfo
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("streetName")]
		public string StreetName { get; set; }

		[JsonProperty("houseNumber")]
		public string HouseNumber { get; set; }
	}
}