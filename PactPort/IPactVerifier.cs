using System;
using System.Collections.Generic;
using Pact.Provider.Wrapper.PactPort.RequestFilters;
using Pact.Provider.Wrapper.Verification;

namespace Pact.Provider.Wrapper.PactPort
{
    public interface IPactVerifier:IDisposable
    {

        void AddRequestFilters(IEnumerable<RequestFilter> filters);
        
        void AddProviderStateSettleActions(Dictionary<string,Action> settleActions); 
        List<PactnetVerificationResult> Verify(Models.Pact pact);

    }
}