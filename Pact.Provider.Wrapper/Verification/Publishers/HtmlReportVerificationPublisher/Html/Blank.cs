namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class Blank:IHyperTextObject
    {
        public string ToHtml()
        {
            return "";
        }

        public IHyperTextObject Attribute(string name, string value)
        {
            return this;
        }
    }
}