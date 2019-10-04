// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameApiResponse.cs" company="Clued In">
//   Copyright (c) 2018 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the name API response class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
    public class NameApiResponse
    {
        public NameApiResponse(string name, string email, NameParser nameParser, DisposableEmailParserResponse disposableEmailParser, RiskDetectionResponse riskDetection)
        {
            Name = name;
            Email = email;
            NameParser = nameParser;
            DisposableEmailParser = disposableEmailParser;
            RiskDetection = riskDetection;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public NameParser NameParser { get; set; }
        public DisposableEmailParserResponse DisposableEmailParser { get; set; }
        public RiskDetectionResponse RiskDetection { get; set; }
    }
}
