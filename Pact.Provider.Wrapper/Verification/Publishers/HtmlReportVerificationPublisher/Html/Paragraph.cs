namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class Paragraph : ContainerBase
    {
        public Paragraph(params IHyperTextObject[] childs) : base(childs)
        {
        }

        protected override string Tag()
        {
            return "p";
        }
    }
}