namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class TableCell:ContainerBase
    {
        public TableCell(params IHyperTextObject[] childs) : base(childs)
        {
        }

        protected override string Tag()
        {
            return "td";
        }
    }
}