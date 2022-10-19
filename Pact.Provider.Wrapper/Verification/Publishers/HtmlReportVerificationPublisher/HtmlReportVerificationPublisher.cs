using System.Collections.Generic;
using System.IO;

namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher
{
    public class HtmlReportVerificationPublisher : IVerificationPublisher
    {
        private readonly string _reportFilename;


        public HtmlReportVerificationPublisher(string reportFilename)
        {
            this._reportFilename = reportFilename;
        }
        
        public void Publish(List<VerificationRecord> verificationRecords)
        {
            var html = new SimpleTableReport(verificationRecords);

            string content = html.ToHtml();

            if (File.Exists(_reportFilename))
            {
                File.Delete(_reportFilename);
            }

            File.WriteAllText(_reportFilename, content);
        }
    }
}