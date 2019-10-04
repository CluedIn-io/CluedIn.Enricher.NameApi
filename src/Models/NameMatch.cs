using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class NameMatch
	{
		[JsonProperty("givenNames")]
		public List<Name> GivenNames { get; set; }

		[JsonProperty("surnames")]
		public List<Name> Surnames { get; set; }

		[JsonProperty("confidence")]
		public double Confidence { get; set; }
	}
}