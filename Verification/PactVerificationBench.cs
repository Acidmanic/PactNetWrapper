using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Pact.Provider.Wrapper.IO;
using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.PactnetVerificationPublish;
using Pact.Provider.Wrapper.Reflection;
using Pact.Provider.Wrapper.Unit;
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

        public PactVerificationBench(string serviceUri)
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

            var selectedToTestEndpoints = PickSelectedEndpoints(splitPacts);

            foreach (var ep in selectedToTestEndpoints)
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

        private Dictionary<string, Models.Pact> PickSelectedEndpoints(Dictionary<string, Models.Pact> byEndpoints)
        {
            var skipAll = new AttributeHelper().DeliveredAttributes<SkipAllEndpointsAttribute>();

            if (skipAll.Count > 0)
            {
                return new Dictionary<string, Models.Pact>();
            }

            var addEndpoints =
                new AttributeHelper().DeliveredAttributes<EndpointAttribute>()
                    .Select(ep => ep.RequestPath.NormalizeHttpUri().ToLower()).ToList();

            var selected = new Dictionary<string, Models.Pact>();

            foreach (var keyValuePair in byEndpoints)
            {
                if (addEndpoints.Count == 0 || IsLabeled(keyValuePair.Key, addEndpoints))
                {
                    selected.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            var skipEndpoints = new AttributeHelper().DeliveredAttributes<SkipEndpointAttribute>()
                .Select(ep => ep.RequestPath.NormalizeHttpUri().ToLower()).ToList();

            var removingKeys = new List<string>();
            
            foreach (var keyValuePair in selected)
            {
                var key = keyValuePair.Key.NormalizeHttpUri().ToLower();

                if (skipEndpoints.Contains(key))
                {
                    removingKeys.Add(keyValuePair.Key);
                }
            }
            
            foreach (var removingKey in removingKeys)
            {
                selected.Remove(removingKey);
            }

            return selected;
        }

        private bool IsLabeled(string endpoint, List<string> labeledEndpoints)
        {
            var key = endpoint.NormalizeHttpUri().ToLower();

            return labeledEndpoints.Count == 0 || labeledEndpoints.Contains(key);
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