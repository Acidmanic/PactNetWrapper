using System;
using System.Collections.Generic;
using System.IO;
using Pact.Provider.Wrapper.IO;
using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.PactnetVerificationPublish;
using Pact.Provider.Wrapper.Validation;
using Pact.Provider.Wrapper.Verification.Publishers;
using PactNet;
using PactNet.Infrastructure.Outputters;

namespace Pact.Provider.Wrapper.Verification
{
    public class PactVerificationBench
    {
        private readonly string _serviceUri;
        private BatchVerificationPublisher _publisher;

        public IPactnetVerificationPublish PactnetVerificationPublish { get; set; }

        public PactVerificationBench(string serviceUri
        )
        {
            this._serviceUri = serviceUri;
            this.PactnetVerificationPublish = new NullPactnetVerificationPublish();
        }


        public void Verify(string pactsDirectory)
        {
            var files = Directory.EnumerateFiles(pactsDirectory);

            var pactsToVerifyAgainst = new List<Models.Pact>();

            foreach (var file in files)
            {
                try
                {
                    if (file.ToLower().EndsWith(".json"))
                    {
                        var pact = new Json().Load(file);

                        if (new PactModelValidator().Validate(pact))
                        {
                            pactsToVerifyAgainst.Add(pact);
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

                var verificationRecords = new List<VerificationRecord>();

                pactsToVerifyAgainst.ForEach(p => VerifyAgainst(p, verificationRecords));

                this._publisher.Publish(verificationRecords);

                ThrowExceptionOnFailure(verificationRecords);
            }
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

            foreach (var ep in splitPacts)
            {
                var interactions = ep.Value.Interactions;

                if (interactions != null && interactions.Count > 0)
                {
                    var runningInteraction = pact.Interactions[0];

                    string fileName = Guid.NewGuid().ToString() + ".json";

                    var publishResultViaBroker =
                        PactnetVerificationPublish.InterceptPactBeforeVerification(ep.Value, runningInteraction);

                    new Json().Save(fileName, ep.Value);

                    var verificationRecord = VerifyAgainst(fileName, publishResultViaBroker,
                        ep.Value, runningInteraction);

                    verificationRecords.Add(verificationRecord);

                    try
                    {
                        File.Delete(fileName);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }

        private VerificationRecord VerifyAgainst(string pactFile,
            bool publishResultViaBroker, Models.Pact runningPact, Interaction runningInteraction)
        {
            var verifierConfig = new PactVerifierConfig
            {
                Outputters = new IOutput[] {new ConsoleOutput()},
                PublishVerificationResults = publishResultViaBroker,
                ProviderVersion = "1"
            };
            var verifier = new PactVerifier(verifierConfig);

            verifier.ServiceProvider(runningPact.Provider?.Name, _serviceUri);

            verifier.PactUri(pactFile);

            VerificationRecord record = new VerificationRecord().UpdateFrom(runningPact, runningInteraction);

            try
            {
                verifier.Verify();

                record.Success = true;
            }
            catch (Exception e)
            {
                record.Success = false;

                record.Exception = e;
            }

            return record;
        }

        public BatchVerificationPublisher WithPublishers()
        {
            this._publisher = new BatchVerificationPublisher();

            return _publisher;
        }
    }
}