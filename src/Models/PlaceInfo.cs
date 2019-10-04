using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class PlaceInfo
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("locality")]
		public string Locality { get; set; }

		[JsonProperty("postalCode")]
		public string PostalCode { get; set; }
	}
}