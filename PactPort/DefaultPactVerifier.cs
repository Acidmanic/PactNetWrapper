using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.Verification;

namespace Pact.Provider.Wrapper.PactPort
{
    public class DefaultPactVerifier : IPactVerifier
    {
        private readonly string _serviceUri;

        public DefaultPactVerifier(string serviceUri)
        {
            _serviceUri = serviceUri;
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

            try
            {
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

            success &= VerifyHeaders(expectations.MatchingRules,expectations.Headers, actual.Headers, log);

            success &= VerifyBodies(expectations.MatchingRules, expectations.Body, actual.Content, log);

            return success;
        }

        private bool VerifyBodies(Dictionary<string, MatchingRule> expectationsMatchingRules,
            Hashtable expectationsBody, HttpContent actualContent, PactLogBuilder log)
        {
            var actualJson = actualContent.ReadAsStringAsync().Result;

            var actualHashTable = JsonConvert.DeserializeObject<Hashtable>(actualJson);

            var expectedDic = new DataConvert().Flatten(expectationsBody, "$.body");

            var actualDic = new DataConvert().Flatten(actualHashTable, "$.body");

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

            return matcher.IsMatch(expectationsMatchingRules, expectedHeaders, actualHeaders,log);
        }
        
        private bool VerifyHeaders(Dictionary<string, string> expectations, HttpResponseHeaders actuals,
            PactLogBuilder log)
        {
            var success = true;

            foreach (var headersKey in expectations.Keys)
            {
                var header = SearchForKey(headersKey, actuals);

                if (header.Key == null)
                {
                    log.NotFound(headersKey);

                    success = false;
                }
                else
                {
                    var expectedHeader = expectations[headersKey];

                    var actualHeader = header.Value;

                    var enumerable = actualHeader as string[] ?? actualHeader.ToArray();

                    if (!enumerable.Contains(expectedHeader))
                    {
                        var values = "";

                        foreach (var s in enumerable)
                        {
                            values += s + "; ";
                        }

                        //LogAssert($"{headersKey}:{expectedHeader}", $"{header.Value}:{values}", log);

                        success = false;
                    }
                }
            }

            return success;
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

            HttpRequestMessage request = new HttpRequestMessage(req.Method, req.Path);


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