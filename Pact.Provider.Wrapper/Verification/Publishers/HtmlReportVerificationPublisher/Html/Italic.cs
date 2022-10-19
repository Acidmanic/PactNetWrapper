namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class Italic : WrapperBase
    {
        public Italic(IHyperTextObject wrappedObject) : base(wrappedObject)
        {
        }

        protected override string Tag()
        {
            return "i";
        }
    }
}