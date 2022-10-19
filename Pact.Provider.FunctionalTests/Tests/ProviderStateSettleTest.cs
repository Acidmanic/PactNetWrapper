using System;
using System.Collections;
using System.Net;
using Pact.Provider.Wrapper.Unit;
using Pact.Provider.Wrapper.Verification;
using Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher;

namespace Pact.Provider.FunctionalTests.Tests
{
    public class ProviderStateSettleTest
    {

        [Endpoint("UserInformation/Noghl")]
        public void Main()
        {
            using var server = new Server<Startup>(9222);

            var bench = new PactVerificationBench("http://localhost:9222");

            bench
                .SettleProvider(".*noghl.*", r =>
                {
                    r.Headers["Authorization"] = "Bearer mearer!";

                    Console.WriteLine("AUTH:" + r.Headers["Authorization"]);
                })
                .UseInternalPactVerifier()
                .WithPublishers()
                .Add(new HtmlReportVerificationPublisher("Report.html"));

            
            bench.Verify("../../../../Pacts");
        }
    }
}