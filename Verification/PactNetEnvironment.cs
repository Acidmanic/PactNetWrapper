using System;
using System.Collections.Generic;
using System.IO;
using Pact.Provider.Wrapper.IO;
using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Mocks.MockHttpService.Models;
using IPactVerifier = Pact.Provider.Wrapper.PactPort.IPactVerifier;

namespace Pact.Provider.Wrapper.Verification
{
    public class PactNetEnvironment : IDisposable, IPactVerifier
    {
        private string _jsonFileName;
        private string _serviceUri;
        private bool _publish;

        public PactNetEnvironment(string serviceUri, bool publish)
        {
            this._serviceUri = serviceUri;
            _jsonFileName = Guid.NewGuid().ToString() + ".json";
            _publish = publish;
        }

        public List<PactnetVerificationResult> Verify(Models.Pact pact)
        {
            SilentKill();

            new Json().Save(_jsonFileName, pact);

            return new List<PactnetVerificationResult>() {VerifyUsingPactNet(_jsonFileName, _publish, pact)};
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