using System;
using System.Collections.Generic;
using Pact.Provider.Wrapper.Verification;

namespace Pact.Provider.Wrapper.PactPort
{
    public interface IPactVerifier:IDisposable
    {

        List<PactnetVerificationResult> Verify(Models.Pact pact);

    }
}