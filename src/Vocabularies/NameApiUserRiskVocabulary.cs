// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameApiEmailAddressRiskVocabulary.cs" company="Clued In">
//   Copyright (c) 2018 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the name API email address risk vocabulary class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.NameApi.Vocabularies
{
    public class NameApiUserRiskVocabulary : DynamicVocabulary
    {
        public NameApiUserRiskVocabulary()
        {
            this.VocabularyName        = "NameApi Person Risk Custom Properties";
            this.KeyPrefix             = "nameApi.user.risk.custom";
            this.KeySeparator          = "-";
            this.Grouping              = EntityType.Unknown;
            this.ShowInApplication     = true;
            this.ShowUrisInApplication = false;
        }
    }
}