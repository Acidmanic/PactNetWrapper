using System;
using System.Collections;
using System.Net;
using Pact.Provider.Wrapper.Unit;
using Pact.Provider.Wrapper.Verification;
using Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher;

namespace Pact.Provider.FunctionalTests.Tests
{
    public class AuthorizeRequestFilter
    {

        [Endpoint("UserInformation/AuthParam")]
        public void Main()
        {
            using var server = new Server<Startup>(9222);

            var bench = new PactVerificationBench("http://localhost:9222");

            bench
                .UseInternalPactVerifier()
                .WithPublishers()
                .Add(new HtmlReportVerificationPublisher("Report.html"));

            bench.WithRequestFilters()
                .Add()
                .Put("radkonbere")
                .At("$.headers.authorization")
                .WithRequestPathUnder("UserInformation");


            bench.Verify("../../../../Pacts");
        }
    }
}