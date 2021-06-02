using System;

namespace Pact.Provider.Wrapper.Verification
{
    public class PactnetVerificationResult
    {
        public string Logs { get; set; }
        
        public Exception Exception { get; set; }
        
        public bool Success { get; set; }
    }
}