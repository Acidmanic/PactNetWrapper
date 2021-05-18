namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class Bold : WrapperBase
    {
        public Bold(IHyperTextObject wrappedObject) : base(wrappedObject)
        {
        }

        protected override string Tag()
        {
            return "b";
        }
    }
}