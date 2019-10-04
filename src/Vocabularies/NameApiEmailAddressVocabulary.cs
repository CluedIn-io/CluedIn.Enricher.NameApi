// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameApiEmailAddressVocabulary.cs" company="Clued In">
//   Copyright (c) 2018 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the name API email address vocabulary class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.NameApi.Vocabularies
{
    public class NameApiEmailAddressVocabulary : SimpleVocabulary
    {        
        public NameApiEmailAddressVocabulary()
        {
            this.VocabularyName = "NameApi Email";
            this.KeyPrefix      = "nameApi.emailAddress";
            this.KeySeparator   = ".";
            this.Grouping       = EntityType.Infrastructure.User;

            this.AddGroup("NameAPI Email Address", group => 
            {
                this.Name            = group.Add(new VocabularyKey("name"));
                this.Email           = group.Add(new VocabularyKey("email"));
                this.Confidence      = group.Add(new VocabularyKey("confidence",        VocabularyKeyDataType.Number));
                this.DisposableEmail = group.Add(new VocabularyKey("disposableEmail",   VocabularyKeyDataType.Boolean));
            });

            this.AddMapping(this.Name, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.FullName);
            this.AddMapping(this.Email, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.Email);
        }

        public VocabularyKey Name { get; internal set; }
        public VocabularyKey Email { get; internal set; }
        public VocabularyKey Confidence { get; internal set; }
        public VocabularyKey DisposableEmail { get; internal set; }
    }
}
