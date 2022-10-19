namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class Html:IHyperTextObject
    {
        
        public Head Head { get; set; }
        
        public Body Body { get; set; }
        
        public string ToHtml()
        {
            return $"<!DOCTYPE html><html>{this.Head.ToHtml()}{this.Body.ToHtml()}</html>";
        }

        public IHyperTextObject Attribute(string name, string value)
        {
            return this;
        }
    }
}