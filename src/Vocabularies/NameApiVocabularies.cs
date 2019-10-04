// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameApiVocabularies.cs" company="Clued In">
//   Copyright (c) 2018 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the name API vocabularies class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CluedIn.ExternalSearch.Providers.NameApi.Vocabularies
{
    public static class NameApiVocabularies
    {
        public static NameApiEmailAddressVocabulary EmailAddress { get; } = new NameApiEmailAddressVocabulary();
        public static NameApiUserRiskVocabulary UserRisk { get; } = new NameApiUserRiskVocabulary();
    }
}
