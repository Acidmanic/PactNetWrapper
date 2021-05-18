using System;
using System.Net.Http;
using Pact.Provider.Wrapper.Models.Augment;

namespace Pact.Provider.Wrapper.Verification
{
    public class VerificationRecord
    {
        public Exception Exception { get; set; }

        public bool Success { get; set; }
        
        public InteractionInfo Interaction { get; set; }
    }
}