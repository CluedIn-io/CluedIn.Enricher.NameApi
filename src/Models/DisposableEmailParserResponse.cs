using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class DisposableEmailParserResponse
	{
		[JsonProperty("disposable")]
		public string Disposable { get; set; }
	}
}