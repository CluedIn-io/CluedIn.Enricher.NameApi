using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class AddressElement
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("address")]
		public Address Address { get; set; }
	}
}