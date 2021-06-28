using System;
using System.Collections.Generic;
using Pact.Provider.Wrapper.PactPort.RequestFilters;
using Pact.Provider.Wrapper.Verification;

namespace Pact.Provider.Wrapper.PactPort
{
    public interface IPactVerifier:IDisposable
    {

        void AddRequestFilters(IEnumerable<RequestFilter> filters); 
        List<PactnetVerificationResult> Verify(Models.Pact pact);

    }
}