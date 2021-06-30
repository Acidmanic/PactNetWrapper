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
        private readonly IInteractionTagger _tagger = new ColumnDelimitedInteractionTagger();

        public AcidmanicPactBrokerPublisher(string brokerUrl, string token)
        {
            _brokerUrl = brokerUrl;
            _token = token;
        }


        public void Publish(List<VerificationRecord> verificationRecords)
        {
            var recordsByTag = GroupByTag(verificationRecords);

            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var tag in recordsByTag.Keys)
            {
                result[tag] = "Success";

                var group = recordsByTag[tag].ToList();

                foreach (var record in group)
                {
                    if (!record.Success)
                    {
                        result[tag] = "Failure";
                        break;
                    }
                }
            }

            Publish(result);
        }

        private Dictionary<string, List<VerificationRecord>> GroupByTag(List<VerificationRecord> verificationRecords)
        {
            Dictionary<string, List<VerificationRecord>> grouped = new Dictionary<string, List<VerificationRecord>>();

            foreach (var record in verificationRecords)
            {
                string tag = _tagger.Tag(record.Interaction);

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