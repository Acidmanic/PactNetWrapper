namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class Text : IHyperTextObject
    {
        public Text()
        {
        }

        public Text(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public string ToHtml()
        {
            return this.Value;
        }

        public IHyperTextObject Attribute(string name, string value)
        {
            return this;
        }
    }
}