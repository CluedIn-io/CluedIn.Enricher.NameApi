using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class NameParserResponse
	{
		[JsonProperty("resultType")]
		public string ResultType { get; set; }

		[JsonProperty("nameMatches")]
		public List<NameMatch> NameMatches { get; set; }
	}
}