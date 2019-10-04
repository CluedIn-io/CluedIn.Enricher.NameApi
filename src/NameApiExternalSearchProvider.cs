// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameApiExternalSearchProvider.cs" company="Clued In">
//   Copyright (c) 2018 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the name API external search provider class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// TODO: This external search provider requires payment - The free api-key only allows ~2500 calls per month.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Crawling.Helpers;
using CluedIn.ExternalSearch.Providers.NameApi.Models;
using CluedIn.ExternalSearch.Providers.NameApi.Vocabularies;

using RestSharp;
using Newtonsoft.Json;
 
namespace CluedIn.ExternalSearch.Providers.NameApi
{
    /// <summary>The nameapi graph external search provider.</summary>
    /// <seealso cref="CluedIn.ExternalSearch.ExternalSearchProviderBase" />
    public class NameApiExternalSearchProvider : ExternalSearchProviderBase
    {
        /**********************************************************************************************************
         * FIELDS
         **********************************************************************************************************/

        public static readonly Guid ProviderId = Guid.Parse("c68f2b2a-aaf8-45ff-a303-c3acd5ffa4a1");   // TODO: Replace value
        private string apiKey = "d30c0a21d88f6eb9a30d6a113473ed7f-user1";

        /**********************************************************************************************************
         * CONSTRUCTORS
         **********************************************************************************************************/

        public NameApiExternalSearchProvider()
            : base(ProviderId, EntityType.Infrastructure.User, EntityType.Infrastructure.Contact, EntityType.Person)
        {
        }
 
        /**********************************************************************************************************
         * METHODS
         **********************************************************************************************************/
 
        /// <summary>Builds the queries.</summary>
        /// <param name="context">The context.</param>
        /// <param name="request">The request.</param>
        /// <returns>The search queries.</returns>
        public override IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request)
        {
            if (!this.Accepts(request.EntityMetaData.EntityType))
                yield break;
  
            var entityType       = request.EntityMetaData.EntityType;

            var email        = request.QueryParameters.GetValue(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.Email, new HashSet<string>());
            var phoneNumber  = request.QueryParameters.GetValue(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.PhoneNumber, new HashSet<string>());
            var streetName   = request.QueryParameters.GetValue(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.HomeAddressStreetName, new HashSet<string>());
            var streetNumber = request.QueryParameters.GetValue(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.HomeAddressStreetNumber, new HashSet<string>());
            var city         = request.QueryParameters.GetValue(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.HomeAddressCity, new HashSet<string>());
            var postalCode   = request.QueryParameters.GetValue(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInUser.HomeAddressZipCode, new HashSet<string>());

            var name = new HashSet<string>();
            if (!string.IsNullOrEmpty(request.EntityMetaData.Name))
                name.Add(request.EntityMetaData.Name);
            if (!string.IsNullOrEmpty(request.EntityMetaData.DisplayName))
                name.Add(request.EntityMetaData.DisplayName);

            var person = new Dictionary<string, string>
            {
                { "name",           name.FirstOrDefault() },
                { "email",          email.FirstOrDefault() },
                { "phoneNumber",    phoneNumber.FirstOrDefault() },
                { "streetName",     streetName.FirstOrDefault() },
                { "streetNumber",   streetNumber.FirstOrDefault() },
                { "city",           city.FirstOrDefault() },
                { "postalCode",     postalCode.FirstOrDefault() }
            };

            // TODO: Split query into multiple queries
            //  1 : for each email, not just the first one emit a query that performs the emailnameparser & disposableemailaddressdetector
            //  2 : separate query for riskdetector requests

            if (email.Any() || (name.Any() && phoneNumber.Any()))
                yield return new ExternalSearchQuery(this, entityType, person);
        }
 
        /// <summary>Executes the search.</summary>
        /// <param name="context">The context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The results.</returns>
        public override IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query)
        {
            string name = "";
            string email = "";
            string phoneNumber = "";
            string streetName = "";
            string streetNumber = "";
            string city = "";
            string postalCode = "";

            if (query.QueryParameters.ContainsKey("name"))
                name = query.QueryParameters["name"].FirstOrDefault();

            if (query.QueryParameters.ContainsKey("email"))
                email = query.QueryParameters["email"].FirstOrDefault();

            if (query.QueryParameters.ContainsKey("phoneNumber"))
                phoneNumber = query.QueryParameters["phoneNumber"].FirstOrDefault();

            if (query.QueryParameters.ContainsKey("streetName"))
                streetName = query.QueryParameters["streetName"].FirstOrDefault();

            if (query.QueryParameters.ContainsKey("streetNumber"))
                streetNumber = query.QueryParameters["streetNumber"].FirstOrDefault();

            if (query.QueryParameters.ContainsKey("city"))
                city = query.QueryParameters["city"].FirstOrDefault();

            if (query.QueryParameters.ContainsKey("postalCode"))
                postalCode = query.QueryParameters["postalCode"].FirstOrDefault();

            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(name))
                yield break;

            var client = new RestClient("http://api.nameapi.org/rest/v5.3/");

            NameParser                      nameFromEmail   = null;
            DisposableEmailParserResponse   disposableEmail = null;
            RiskDetectionResponse           riskDetection   = null;

            if (!string.IsNullOrEmpty(email))
            {
                if (string.IsNullOrEmpty(name))
                {
                    var nameParserResponse = RequestWrapper<NameParserResponse>(client, String.Format("email/emailnameparser?apiKey={0}&emailAddress={1}", apiKey, email), Method.GET);

                    // TODO: This should not be done here - the result should contain the raw response from the service
                    var nameMatches = nameParserResponse.NameMatches.FirstOrDefault();
                    var givenNames  = String.Join(" ", nameMatches.GivenNames.Select(x => x.Value).ToArray());
                    var surnames    = String.Join(" ", nameMatches.Surnames.Select(x => x.Value).ToArray());

                    name = String.Format("{0} {1}", givenNames, surnames);
                    nameFromEmail = new NameParser(name, nameMatches.Confidence);
                }

                disposableEmail = RequestWrapper<DisposableEmailParserResponse>(client, String.Format("email/disposableemailaddressdetector?apiKey={0}&emailAddress={1}", apiKey, email), Method.GET);
            }

            if (!string.IsNullOrEmpty(name))
            {
                var person = new RiskDetectionRequest(
                    name: name,
                    phoneNumber: phoneNumber,
                    emailAddress: email,
                    streetName: streetName,
                    streetNumber: streetNumber,
                    city: city,
                    postalCode: postalCode
                );

                riskDetection = RequestWrapper<RiskDetectionResponse>(client, String.Format("riskdetector/person?apiKey={0}", apiKey), Method.POST, request =>
                {
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("Application/Json", JsonConvert.SerializeObject(person), ParameterType.RequestBody);
                });
            }

            if (nameFromEmail != null || disposableEmail != null || riskDetection != null)
            {
                var nameApiResponse = new NameApiResponse(name, email, nameFromEmail, disposableEmail, riskDetection);

                yield return new ExternalSearchQueryResult<NameApiResponse>(query, nameApiResponse);
            }
        }
 
        /// <summary>Builds the clues.</summary>
        /// <param name="context">The context.</param>
        /// <param name="query">The query.</param>
        /// <param name="result">The result.</param>
        /// <param name="request">The request.</param>
        /// <returns>The clues.</returns>
        public override IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            var resultItem = result.As<NameApiResponse>();
 
            var code = this.GetOriginEntityCode(resultItem);
 
            var clue = new Clue(code, context.Organization);
 
            this.PopulateMetadata(clue.Data.EntityData, resultItem);

            return new[] { clue };
        }
 
        /// <summary>Gets the primary entity metadata.</summary>
        /// <param name="context">The context.</param>
        /// <param name="result">The result.</param>
        /// <param name="request">The request.</param>
        /// <returns>The primary entity metadata.</returns>
        public override IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            var resultItem = result.As<NameApiResponse>();

            return this.CreateMetadata(resultItem);
        }

        /// <summary>Gets the preview image.</summary>
        /// <param name="context">The context.</param>
        /// <param name="result">The result.</param>
        /// <param name="request">The request.</param>
        /// <returns>The preview image.</returns>
        public override IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            return null;
        }
 
        /// <summary>Creates the metadata.</summary>
        /// <param name="resultItem">The result item.</param>
        /// <returns>The metadata.</returns>
        private IEntityMetadata CreateMetadata(IExternalSearchQueryResult<NameApiResponse> resultItem)
        {
            var metadata = new EntityMetadataPart();
 
            this.PopulateMetadata(metadata, resultItem);
 
            return metadata;
        }
 
        /// <summary>Gets the origin entity code.</summary>
        /// <param name="resultItem">The result item.</param>
        /// <returns>The origin entity code.</returns>
        private EntityCode GetOriginEntityCode(IExternalSearchQueryResult<NameApiResponse> resultItem)
        {
            return new EntityCode(EntityType.Infrastructure.User, this.GetCodeOrigin(), resultItem.Data.Email);
        }
 
        /// <summary>Gets the code origin.</summary>
        /// <returns>The code origin</returns>
        private CodeOrigin GetCodeOrigin()
        {
            return CodeOrigin.CluedIn.CreateSpecific("nameapi");
        }
 
        /// <summary>Populates the metadata.</summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="resultItem">The result item.</param>
        private void PopulateMetadata(IEntityMetadata metadata, IExternalSearchQueryResult<NameApiResponse> resultItem)
        {
            var code  = this.GetOriginEntityCode(resultItem);

            var result = resultItem.Data;

            var riskDetection = result.RiskDetection;

            metadata.EntityType       = EntityType.Infrastructure.User;
            metadata.Name             = result.Name;
            metadata.OriginEntityCode = code;
            
            metadata.Codes.Add(code);

            metadata.Properties[NameApiVocabularies.EmailAddress.Name]            = result.NameParser?.Name.PrintIfAvailable();
            metadata.Properties[NameApiVocabularies.EmailAddress.Email]           = result.Email;
            metadata.Properties[NameApiVocabularies.EmailAddress.Confidence]      = result.NameParser?.Confidence.PrintIfAvailable();
            metadata.Properties[NameApiVocabularies.EmailAddress.DisposableEmail] = result.DisposableEmailParser?.Disposable.PrintIfAvailable();

            if (riskDetection != null)
            {
                var risks = riskDetection.Risks;
                for (int i = 0; i < risks.Count; i++)
                {
                    metadata.Properties[NameApiVocabularies.UserRisk.KeyPrefix + NameApiVocabularies.UserRisk.KeySeparator + $"{i}.reason"] = risks[i].Reason.PrintIfAvailable();
                    metadata.Properties[NameApiVocabularies.UserRisk.KeyPrefix + NameApiVocabularies.UserRisk.KeySeparator + $"{i}.item"]   = risks[i].DataItem.PrintIfAvailable();
                    metadata.Properties[NameApiVocabularies.UserRisk.KeyPrefix + NameApiVocabularies.UserRisk.KeySeparator + $"{i}.score"]  = risks[i].RiskScore.PrintIfAvailable();
                }
            }
        }

        /// <summary>
        /// Wrapper around a request to ensure propper deserialization of the Json.
        /// </summary>
        /// <typeparam name="T">The model</typeparam>
        /// <param name="client">An IRestClient for the request</param>
        /// <param name="parameter">The parameters for the request</param>
        /// <param name="callback">Pass additional information to the request (e.g. headers)</param>
        /// <returns>A deserialized object</returns>
        public static T RequestWrapper<T>(IRestClient client, string parameter, Method method, Action<IRestRequest> callback = null)
        {
            var request = new RestRequest(parameter, method);

            callback?.Invoke(request);

            var response = client.ExecuteTaskAsync(request).Result;

            T responseData;

            if (response.StatusCode == HttpStatusCode.OK)
                responseData = JsonConvert.DeserializeObject<T>(response.Content);
            else if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
                responseData = default(T);
            else if (response.ErrorException != null)
                throw new AggregateException(response.ErrorException.Message, response.ErrorException);
            else
                throw new ApplicationException("Could not execute external search query - StatusCode:" + response.StatusCode + "; Content: " + response.Content);

            return responseData;
        }
    }
}