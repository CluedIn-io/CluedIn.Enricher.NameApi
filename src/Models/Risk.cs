using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class Risk
	{
		[JsonProperty("dataItem")]
		public string DataItem { get; set; }

		[JsonProperty("riskScore")]
		public double RiskScore { get; set; }

		[JsonProperty("reason")]
		public string Reason { get; set; }

		[JsonProperty("riskType")]
		public List<string> RiskType { get; set; }
	}
}