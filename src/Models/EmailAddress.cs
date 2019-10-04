using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class EmailAddress
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("emailAddress")]
		public string Address { get; set; }
	}
}