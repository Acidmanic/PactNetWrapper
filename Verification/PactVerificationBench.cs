using System;
using System.Collections.Generic;
using Pact.Provider.Wrapper.IO;
using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.PactnetVerificationPublish;
using Pact.Provider.Wrapper.PactPort;
using Pact.Provider.Wrapper.Reflection;
using Pact.Provider.Wrapper.Verification.Publishers;

namespace Pact.Provider.Wrapper.Verification
{
    public class PactVerificationBench
    {
        private readonly string _serviceUri;
        private BatchVerificationPublisher _publisher;
        private Func<bool,IPactVerifier> _pactVerifierFactory;
        public IPactnetVerificationPublish PactnetVerificationPublish { get; set; }

        public PactVerificationBench(string serviceUri)
        {
            this._serviceUri = serviceUri;
            this.PactnetVerificationPublish = new NullPactnetVerificationPublish();
            
        }

        public PactVerificationBench UsePactNet()
        {
            _pactVerifierFactory = (publish) => new  PactNetEnvironment(_serviceUri,publish);

            return this;
        }

        public PactVerificationBench UseInternalPactVerifier()
        {
            _pactVerifierFactory = _ => new  DefaultPactVerifier(_serviceUri);

            return this;
        }

        public void Verify(string pactsDirectory)
        {
            var pactsToVerifyAgainst = new Json().LoadDirectory(pactsDirectory);

            var verificationRecords = new List<VerificationRecord>();

            pactsToVerifyAgainst.ForEach(p => VerifyAgainst(p, verificationRecords));

            this._publisher.Publish(verificationRecords);

            ThrowExceptionOnFailure(verificationRecords);
        }

        private void ThrowExceptionOnFailure(List<VerificationRecord> verificationRecords)
        {
            foreach (var record in verificationRecords)
            {
                if (!record.Success)
                {
                    if (record.Exception != null)
                    {
                        throw record.Exception;
                    }

                    throw new Exception("Failed to pass tests for interaction: " + record.DescribeInteraction());
                }
            }
        }

        private void VerifyAgainst(Models.Pact pact, List<VerificationRecord> verificationRecords)
        {
            var splitPacts = pact.SplitByEndpoint();

            var selectedToTestEndpoints = new EndpointFilter().Filter(splitPacts);

            foreach (var ep in selectedToTestEndpoints)
            {
                var singleInteractions = ep.Value.SplitByInteractions();

                foreach (var singleInteraction in singleInteractions)
                {
                    var interaction = singleInteraction.Interactions[0];

                    var publishResultViaBroker =
                        PactnetVerificationPublish.InterceptPactBeforeVerification(singleInteraction, interaction);

                    var verificationRecord = new VerificationRecord().UpdateFrom(singleInteraction, interaction);

                    using (var pactnet = _pactVerifierFactory(publishResultViaBroker))
                    {
                        var result = pactnet.Verify(singleInteraction);
                    
                        verificationRecord.UpdateFrom(result[0]);
                    }
                    
                    verificationRecords.Add(verificationRecord);
                }
            }
        }

        public BatchVerificationPublisher WithPublishers()
        {
            this._publisher = new BatchVerificationPublisher();

            return _publisher;
        }
    }
}