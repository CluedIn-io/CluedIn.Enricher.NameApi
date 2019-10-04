using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class TelNumber
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("fullNumber")]
		public string FullNumber { get; set; }

	}
}