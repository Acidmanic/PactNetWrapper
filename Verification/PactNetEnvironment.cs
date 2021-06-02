using System;
using System.IO;
using Pact.Provider.Wrapper.IO;
using PactNet;
using PactNet.Infrastructure.Outputters;

namespace Pact.Provider.Wrapper.Verification
{
    public class PactNetEnvironment : IDisposable
    {
        private string _jsonFileName;
        private string _serviceUri;

        public PactNetEnvironment(string serviceUri)
        {
            this._serviceUri = serviceUri;
            _jsonFileName = Guid.NewGuid().ToString() + ".json";
        }

        public PactnetVerificationResult IsolateVerify(Models.Pact pact, bool publish)
        {
            SilentKill();

            new Json().Save(_jsonFileName, pact);

            return VerifyUsingPactNet(_jsonFileName, publish, pact);
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