using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class PersonName
	{
		[JsonProperty("nameFields")]
		public List<NameField> NameFields { get; set; }
	}
}