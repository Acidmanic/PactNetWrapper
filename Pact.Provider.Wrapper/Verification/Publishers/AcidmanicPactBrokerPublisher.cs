using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace Pact.Provider.Wrapper.Verification.Publishers
{
    public class AcidmanicPactBrokerPublisher : IVerificationPublisher
    {
        private readonly string _brokerUrl;
        private readonly string _token;
        private readonly IPublicationTagger _tagger = new ColumnDelimitedPublicationTagger();


        public AcidmanicPactBrokerPublisher(string brokerUrl, string token)
        {
            _brokerUrl = brokerUrl;
            _token = token;
        }


        public void Publish(List<VerificationRecord> verificationRecords)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            AddByServices(verificationRecords, result);

            AddByEndpoints(verificationRecords, result);

            AddByInteractions(verificationRecords, result);

            Publish(result);
        }

        private void AddByServices(List<VerificationRecord> verificationRecords, Dictionary<string, string> result)
        {
            var validations = new Dictionary<string, bool>();

            foreach (var verificationRecord in verificationRecords)
            {
                string serviceTag = _tagger.TagService(verificationRecord.Interaction);

                if (!validations.ContainsKey(serviceTag))
                {
                    validations.Add(serviceTag, true);
                }

                validations[serviceTag] = validations[serviceTag] && verificationRecord.Success;
            }

            foreach (var keyValuePair in validations)
            {
                AddConsideringNotUniqueTags(keyValuePair.Key, keyValuePair.Value, result);
            }
        }

        /// <summary>
        /// This method will add a tag into the results, but first checks if the tag already exists,
        /// the result would be the logical AND of existing and incoming values  
        /// </summary>
        private void AddConsideringNotUniqueTags(string tag, bool success, Dictionary<string, string> result)
        {
            var overallSuccess = success;

            if (result.ContainsKey(tag))
            {
                var existing = result[tag] == "Success";

                overallSuccess &= existing;

                result.Remove(tag);
            }

            result.Add(tag, overallSuccess ? "Success" : "Failure");
        }

        private void AddByInteractions(List<VerificationRecord> verificationRecords, Dictionary<string, string> result)
        {
            foreach (var verificationRecord in verificationRecords)
            {
                string interactionTag = _tagger.TagInteraction(verificationRecord.Interaction);

                if (result.ContainsKey(interactionTag))
                {
                    result.Remove(interactionTag);
                }
                AddConsideringNotUniqueTags(interactionTag, verificationRecord.Success, result);
            }
        }

        private void AddByEndpoints(List<VerificationRecord> verificationRecords, Dictionary<string, string> result)
        {
            Dictionary<string, bool> endpointResults = new Dictionary<string, bool>();

            foreach (var verificationRecord in verificationRecords)
            {
                string endpointTag = _tagger.TagEndpoint(verificationRecord.Interaction);

                if (!endpointResults.ContainsKey(endpointTag))
                {
                    endpointResults.Add(endpointTag, true);
                }

                endpointResults[endpointTag] = endpointResults[endpointTag] && verificationRecord.Success;
            }

            foreach (var keyValuePair in endpointResults)
            {
                AddConsideringNotUniqueTags(keyValuePair.Key, keyValuePair.Value, result);
            }
        }


        private Dictionary<string, List<VerificationRecord>> GroupByTag(List<VerificationRecord> verificationRecords)
        {
            Dictionary<string, List<VerificationRecord>> grouped = new Dictionary<string, List<VerificationRecord>>();

            foreach (var record in verificationRecords)
            {
                string tag = _tagger.TagInteraction(record.Interaction);

                if (!grouped.ContainsKey(tag))
                {
                    grouped.Add(tag, new List<VerificationRecord>());
                }

                grouped[tag].Add(record);
            }

            return grouped;
        }

        private void Publish(Dictionary<string, string> result)
        {
            try
            {
                HttpClient client = new HttpClient();

                string json = JsonConvert.SerializeObject(result);

                HttpContent content = new StringContent(json);

                content.Headers.Clear();

                content.Headers.Add("Content-Type", "application/json");

                content.Headers.Add("token", this._token);

                client.PostAsync(this._brokerUrl, content).Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    this.GetType().Name + ": Unable to publish results because of the the following error:");
                Console.WriteLine(e);
            }
        }
    }
}