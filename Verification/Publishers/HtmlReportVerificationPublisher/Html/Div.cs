namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class Div : ContainerBase
    {
        public Div(params IHyperTextObject[] childs) : base(childs)
        {
        }

        protected override string Tag()
        {
            return "div";
        }
    }
}