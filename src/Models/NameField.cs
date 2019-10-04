using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class NameField
	{
		[JsonProperty("string")]
		public string String { get; set; }

		[JsonProperty("fieldType")]
		public string FieldType { get; set; }
	}
}