using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class InputPerson
	{
		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("emailAddresses")]
		public List<EmailAddress> EmailAddresses { get; set; }

		[JsonProperty("correspondenceLanguage")]
		public string CorrespondenceLanguage { get; set; }

		[JsonProperty("addresses")]
		public List<AddressElement> Addresses { get; set; }

		[JsonProperty("telNumbers")]
		public List<TelNumber> TelNumbers { get; set; }

		[JsonProperty("personName")]
		public PersonName PersonName { get; set; }
	}
}