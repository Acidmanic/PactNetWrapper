using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Pact.Provider.Wrapper.Models.Augment;
using Pact.Provider.Wrapper.Verification;
using Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher;

namespace Pact.Provider.FunctionalTests.Tests
{
    public class TableReportTest
    {
        private class Tagger : IPublicationTagger
        {
            public string TagInteraction(InteractionInfo interaction)
            {
                return "I";
            }

            public string TagEndpoint(InteractionInfo interaction)
            {
                return "EP";
            }

            public string TagService(InteractionInfo interaction)
            {
                return "TS";
            }
        }

        public void Main()
        {
            var tagger = new Tagger();

            var records = new List<VerificationRecord>
            {
                new VerificationRecord
                {
                    Interaction = new InteractionInfo
                    {
                        ProviderState = "Test",
                        RequestPath = "inja/com",
                        RequestMethod = new HttpMethod("Get"),
                        ExpectedStatusCode = 200,
                        ConsumerName = "Client",
                        ProviderName = "Server"
                    },
                    Logs = "It's cool",
                    Success = true,
                    Exception = null
                },
                new VerificationRecord
                {
                    Interaction = new InteractionInfo
                    {
                        ProviderState = "Test",
                        RequestPath = "unja/com",
                        RequestMethod = new HttpMethod("Post"),
                        ExpectedStatusCode = 200,
                        ConsumerName = "Client",
                        ProviderName = "Server"
                    },
                    Logs = "It's not cool",
                    Success = false,
                    Exception = null
                },
                new VerificationRecord
                {
                    Interaction = new InteractionInfo
                    {
                        ProviderState = "Test",
                        RequestPath = "unja/com",
                        RequestMethod = new HttpMethod("Put"),
                        ExpectedStatusCode = 200,
                        ConsumerName = "Client",
                        ProviderName = "Server"
                    },
                    Logs = "It's not cool",
                    Success = true,
                    Exception = null
                },
                new VerificationRecord
                {
                    Interaction = new InteractionInfo
                    {
                        ProviderState = "Test",
                        RequestPath = "unja/com",
                        RequestMethod = new HttpMethod("delete"),
                        ExpectedStatusCode = 200,
                        ConsumerName = "Client",
                        ProviderName = "Server"
                    },
                    Logs = "It's not cool",
                    Success = false,
                    Exception = null
                }
            };

            var table = new SimpleTableReport(records, tagger);

            var html = table.ToHtml();

            File.WriteAllText("report.html", html);
        }
    }
}