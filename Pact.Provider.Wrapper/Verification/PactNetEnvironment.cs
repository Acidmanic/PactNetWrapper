using System;
using System.Collections.Generic;
using System.IO;
using Pact.Provider.Wrapper.IO;
using Pact.Provider.Wrapper.Models;
using Pact.Provider.Wrapper.PactPort;
using Pact.Provider.Wrapper.PactPort.RequestFilters;
using PactNet;
using PactNet.Infrastructure.Outputters;
using IPactVerifier = Pact.Provider.Wrapper.PactPort.IPactVerifier;

namespace Pact.Provider.Wrapper.Verification
{
    public class PactNetEnvironment : IDisposable, IPactVerifier
    {
        private string _jsonFileName;
        private string _serviceUri;
        private bool _publish;
        private IEnumerable<RequestFilter> _filters;

        public PactNetEnvironment(string serviceUri, bool publish)
        {
            this._serviceUri = serviceUri;
            _jsonFileName = Guid.NewGuid().ToString() + ".json";
            _publish = publish;
        }

        public void AddRequestFilters(IEnumerable<RequestFilter> filters)
        {
            _filters = filters;
        }

        public void SetProviderSettlement(ProviderSettlement providerSettlement)
        {
            throw new NotImplementedException("To Use provider state settling feature, please use Builtin verifier.");
        }

     

        public List<PactnetVerificationResult> Verify(Models.Pact pact)
        {
            SilentKill();

            var updatedPact = ApplyRequestFiltersOn(pact);
            
            new Json().Save(_jsonFileName, updatedPact);

            return new List<PactnetVerificationResult>() {VerifyUsingPactNet(_jsonFileName, _publish, updatedPact)};
        }

        private Models.Pact ApplyRequestFiltersOn(Models.Pact pact)
        {
            var updated = new Models.Pact
            {
                _links = pact._links?.Clone(),
                Consumer = new Party() {Name = pact.Consumer?.Name},
                Metadata = new PactMetadata()
                {
                    PactSpecification = new Specification()
                    {
                        Version = pact.Metadata?.PactSpecification?.Version
                    }
                },
                Provider = new Party() {Name = pact.Provider?.Name},
                Interactions = new List<Interaction>()
            };

            var applier = new RequestFilterApplier();
            
            foreach (var pactInteraction in pact.Interactions)
            {
                var interaction = pactInteraction;

                foreach (var filter in _filters)
                {
                    interaction = applier.Apply(filter, interaction);
                }
                
                updated.Interactions.Add(interaction);
            }

            return updated;
        }

        private PactnetVerificationResult VerifyUsingPactNet(string pactFile,
            bool publishResultViaBroker, Models.Pact runningPact)
        {
            var result = new PactnetVerificationResult();

            var logger = new StringAppendOutput();

            var verifierConfig = new PactVerifierConfig
            {
                Outputters = new IOutput[] {logger},
                PublishVerificationResults = publishResultViaBroker,
                ProviderVersion = "1"
            };

            var verifier = new PactVerifier(verifierConfig);

            verifier.ServiceProvider(runningPact.Provider?.Name, _serviceUri);

            verifier.PactUri(pactFile);

            try
            {
                verifier.Verify();

                result.Success = true;
            }
            catch (Exception e)
            {
                result.Success = false;

                result.Exception = e;
            }

            result.Logs = logger.Log;

            return result;
        }

        public void Dispose()
        {
            SilentKill();
        }

        private void SilentKill()
        {
            if (File.Exists(_jsonFileName))
            {
                try
                {
                    File.Delete(_jsonFileName);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}