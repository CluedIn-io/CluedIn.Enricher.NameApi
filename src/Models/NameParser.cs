namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
	public class NameParser
	{
		public NameParser(string name, double confidence)
		{
			Name = name;
			Confidence = confidence;
		}

		public string Name { get; set; }
		public double Confidence { get; set; }
	}
}