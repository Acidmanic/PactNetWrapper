using System.Collections.Generic;

namespace Pact.Provider.Wrapper.Verification.Publishers
{
    public class BatchVerificationPublisher:IVerificationPublisher
    {
        private readonly List<IVerificationPublisher> _items = new List<IVerificationPublisher>();


        public BatchVerificationPublisher Add(IVerificationPublisher publisher)
        {
            this._items.Add(publisher);

            return this;
        }
        
        public void Publish(List<VerificationRecord> verificationRecords)
        {
            this._items.ForEach(p => p.Publish(verificationRecords) );
        }
    }
}