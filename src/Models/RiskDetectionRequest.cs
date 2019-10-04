// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RiskDetectionRequest.cs" company="Clued In">
//   Copyright (c) 2018 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the risk detection request class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.NameApi.Models
{
    public class RiskDetectionRequest
    {
        [JsonProperty("context")]
        public Context Context { get; set; }

        [JsonProperty("inputPerson")]
        public InputPerson InputPerson { get; set; }

        // Create the object that should be serialized to JSON
        public RiskDetectionRequest(string name = "", string phoneNumber = "", string emailAddress = "", string city = "", string postalCode = "", string streetName = "", string streetNumber = "")
        {
            Context = new Context();
            InputPerson = new InputPerson
            {
                Type = "NaturalInputPerson",
                EmailAddresses = new List<EmailAddress>
                {
                    new EmailAddress
                    {
                        Type = "EmailAddressImpl",
                        Address = emailAddress
                    }
                },
                CorrespondenceLanguage = "en",
                Addresses = new List<AddressElement>
                {
                    new AddressElement
                    {
                        Type = "SpecificUsageAddressRelation",
                        Address = new Address
                        {
                            Type = "StructuredAddress",
                            PlaceInfo = new PlaceInfo
                            {
                                Type = "StructuredPlaceInfo",
                                Locality = city,
                                PostalCode = postalCode
                            },
                            StreetInfo = new StreetInfo
                            {
                                Type = "StructuredStreetInfo",
                                StreetName = streetName,
                                HouseNumber = streetNumber
                            }
                        }
                    }
                },
                TelNumbers = new List<TelNumber>
                {
                    new TelNumber
                    {
                        Type = "SimpleTelNumber",
                        FullNumber = phoneNumber
                    }
                },
                PersonName = new PersonName
                {
                    NameFields = new List<NameField>
                    {
                        new NameField
                        {
                            String = name,
                            FieldType = "FULLNAME"
                        }
                    }
                }
            };
        }
    }
}