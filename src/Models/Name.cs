using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class Name
	{
		[JsonProperty("name")]
		public string Value { get; set; }

		[JsonProperty("nameType")]
		public string NameType { get; set; }
	}
}