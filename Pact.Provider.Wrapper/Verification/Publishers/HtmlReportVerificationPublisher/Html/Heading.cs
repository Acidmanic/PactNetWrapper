namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class Heading : ContainerBase
    {
        private readonly int _degree;

        public Heading(params IHyperTextObject[] childs) : base(childs)
        {
        }
        
        public Heading(int degree,params IHyperTextObject[] childs) : base(childs)
        {
            this._degree = degree;
        }

        public Heading(int degree)
        {
            this._degree = degree;
        }

        public Heading()
        {
            this._degree = 1;
        }

        protected override string Tag()
        {
            return "h" + this._degree;
        }
    }
}