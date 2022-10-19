using Pact.Provider.Wrapper.Models;

namespace Pact.Provider.Wrapper.PactnetVerificationPublish
{
    public class NullPactnetVerificationPublish:IPactnetVerificationPublish
    {
        public bool InterceptPactBeforeVerification(Models.Pact pact, Interaction runningInteraction)
        {
            return false;
        }
    }
}