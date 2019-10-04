using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class Address
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("placeInfo")]
		public PlaceInfo PlaceInfo { get; set; }

		[JsonProperty("streetInfo")]
		public StreetInfo StreetInfo { get; set; }
	}
}