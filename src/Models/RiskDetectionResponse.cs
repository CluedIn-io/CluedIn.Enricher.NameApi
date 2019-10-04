// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RiskDetectionResponse.cs" company="Clued In">
//   Copyright (c) 2018 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the risk detection response class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
    public class RiskDetectionResponse
    {
        [JsonProperty("score")]
        public string Score { get; set; }

        [JsonProperty("risks")]
        public List<Risk> Risks { get; set; }
    }
}
