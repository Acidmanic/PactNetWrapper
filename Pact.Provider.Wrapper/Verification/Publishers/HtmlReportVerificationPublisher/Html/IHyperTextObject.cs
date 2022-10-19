namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public interface IHyperTextObject
    {
        string ToHtml();

        IHyperTextObject Attribute(string name, string value);
    }
}