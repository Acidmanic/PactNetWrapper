using System.Collections.Generic;

namespace Pact.Provider.Wrapper.Verification.Publishers
{
    public interface IVerificationPublisher
    {
        void Publish(List<VerificationRecord> verificationRecords);
    }
}