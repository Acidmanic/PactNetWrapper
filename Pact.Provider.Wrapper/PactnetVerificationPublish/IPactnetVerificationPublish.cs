using Pact.Provider.Wrapper.Models;

namespace Pact.Provider.Wrapper.PactnetVerificationPublish
{
    public interface IPactnetVerificationPublish
    {
        /// <summary>
        /// Here it can intercept the pact model for example change the _links values, before verification happens.
        /// It can also determine if result will be published by via Pactnet broker or not by returning true
        /// or false.
        /// </summary>
        /// <param name="pact"></param>
        /// <param name="runningInteraction"></param>
        /// <returns>True will allow Pactnet to publish verification results via pactnet broker.</returns>
        bool InterceptPactBeforeVerification(Models.Pact pact, Interaction runningInteraction);
    }
}