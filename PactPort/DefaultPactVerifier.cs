using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.PactPort.RequestFilters;
using Pact.Provider.Wrapper.Verification;

namespace Pact.Provider.Wrapper.PactPort
{
    public class DefaultPactVerifier : IPactVerifier
    {
        private readonly string _serviceUri;
        private readonly List<RequestFilter> _filters = new List<RequestFilter>();

        private ProviderSettlement _providerSettlement;

        public DefaultPactVerifier(string serviceUri)
        {
            _serviceUri = serviceUri;

            _providerSettlement = new ProviderSettlement(new Dictionary<string, Action>());
        }
        
        public void AddRequestFilters(IEnumerable<RequestFilter> filters)
        {
            _filters.AddRange(filters);
        }

        public void SetProviderSettlement(ProviderSettlement providerSettlement)
        {
            _providerSettlement = providerSettlement;
        }

        public List<PactnetVerificationResult> Verify(Models.Pact pact)
        {
            var result = new List<PactnetVerificationResult>();

            pact.Interactions.ForEach(i => result.Add(Verify(i)));

            return result;
        }


        public PactnetVerificationResult Verify(Interaction interaction)
        {
            PactnetVerificationResult result = new PactnetVerificationResult();

            var logs = new PactLogBuilder();
            
            _providerSettlement.PrepareProvider(interaction);

            try
            {
                interaction = ApplyRequestFilters(interaction);

                HttpRequestMessage request = DesignRequestForInteraction(interaction);

                HttpClient client = new HttpClient {BaseAddress = new Uri(_serviceUri)};

                var response = client.SendAsync(request).Result;

                result.Success = Compare(interaction.Response, response, logs);

                result.Exception = result.Success ? null : new Exception("Unmatched Response Exception");
            }
            catch (Exception e)
            {
                result.Success = false;

                result.Exception = e;
            }

            result.Logs = logs.ToString();

            return result;
        }

        private Interaction ApplyRequestFilters(Interaction interaction)
        {
            var applier = new RequestFilterApplier();

            _filters.ForEach(filter => interaction = applier.Apply(filter, interaction));

            return interaction;
        }

        private bool Compare(PactResponse expectations, HttpResponseMessage actual, PactLogBuilder log)
        {
            var success = true;

            if (expectations.Status != 0)
            {
                if ((int) actual.StatusCode != expectations.Status)
                {
                    log.Unmatched(expectations.Status, actual.StatusCode);

                    success = false;
                }
            }

            var ruleSet = expectations.MatchingRules ?? new Dictionary<string, MatchingRule>();

            success &= VerifyHeaders(ruleSet, expectations.Headers, actual.Headers, log);

            success &= VerifyBodies(ruleSet, expectations.Body, actual.Content, log);

            return success;
        }

        private bool VerifyBodies(Dictionary<string, MatchingRule> expectationsMatchingRules,
            Hashtable expectationsBody, HttpContent actualContent, PactLogBuilder log)
        {
            var actualJson = actualContent.ReadAsStringAsync().Result;

            var actualHashTable = JsonConvert.DeserializeObject<Hashtable>(actualJson);

            var expectedDic = new DynamicObjectAccess.FlatAccess(true).Flatten(expectationsBody, "$.body");

            var actualDic = new DynamicObjectAccess.FlatAccess(true).Flatten(actualHashTable, "$.body");

            var matcher = new Matcher();

            return matcher.IsMatch(expectationsMatchingRules, expectedDic, actualDic, log);
        }


        private bool VerifyHeaders(Dictionary<string, MatchingRule> expectationsMatchingRules,
            Dictionary<string, string> expectations, HttpResponseHeaders actuals,
            PactLogBuilder log)
        {
            var expectedHeaders = new DataConvert().Normalize(expectations);
            var actualHeaders = new DataConvert().Normalize(actuals);

            var matcher = new Matcher();

            return matcher.IsMatch(expectationsMatchingRules, expectedHeaders, actualHeaders, log);
        }

        private KeyValuePair<string, IEnumerable<string>> SearchForKey(string findingKey,
            HttpResponseHeaders actualHeaders)
        {
            var lowKey = findingKey.ToLower();

            foreach (var header in actualHeaders)
            {
                if (header.Key.ToLower() == lowKey)
                {
                    return header;
                }
            }

            return new KeyValuePair<string, IEnumerable<string>>(null, null);
        }

        private HttpRequestMessage DesignRequestForInteraction(Interaction interaction)
        {
            var req = interaction.Request;

            var query = HttpUtility.ParseQueryString(req.Query ?? string.Empty);

            var uri = req.Path +  (query.Count > 0 ? "?" + query : string.Empty);
            
            HttpRequestMessage request = new HttpRequestMessage(req.Method,uri);

            if (req.Body != null && req.Body.Count > 0)
            {
                var json = JsonConvert.SerializeObject(req.Body);

                string contentType = ReadContentType(req.Headers);

                HttpContent content = new StringContent(json, Encoding.Default, contentType);

                request.Content = content;
            }

            foreach (var keyValuePair in req.Headers)
            {
                request.Headers.TryAddWithoutValidation(keyValuePair.Key, keyValuePair.Value);
            }

            return request;
        }

        private string ReadContentType(Dictionary<string, string> reqHeaders)
        {
            foreach (var reqHeadersKey in reqHeaders.Keys)
            {
                if (reqHeadersKey.ToLower() == "content-type")
                {
                    return reqHeaders[reqHeadersKey];
                }
            }

            return "application/json";
        }

        public void Dispose()
        {
        }
    }
}