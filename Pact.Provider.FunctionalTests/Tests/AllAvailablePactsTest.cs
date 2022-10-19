using Pact.Provider.Wrapper.Verification;
using Pact.Provider.Wrapper.Verification.Publishers;
using Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher;

namespace Pact.Provider.FunctionalTests.Tests
{
    public class AllAvailablePactsTest
    {
        public void Main()
        {
            using var server = new Server<Startup>(9222);

            var bench = new PactVerificationBench("http://localhost:9222");

            bench
                .UseInternalPactVerifier()
                .WithPublishers()
                .Add(new AcidmanicPactBrokerPublisher("http://5.160.179.226:13799/badges","5ffff2af-1ab9-416d-b3fc-15493a5271ec.33ac0ff9-e48a-4530-ba6a-3071aabfa4a0"))
                .Add(new HtmlReportVerificationPublisher("Report.html"));

            
            bench.Verify("../../../../Pacts");
        }
    }
}