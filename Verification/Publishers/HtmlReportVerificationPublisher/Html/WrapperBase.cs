using System.Text;

namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public abstract class WrapperBase : AttributableBase
    {
        public IHyperTextObject WrappedObject { get; }

        protected WrapperBase(IHyperTextObject wrappedObject)
        {
            WrappedObject = wrappedObject;
        }

        protected WrapperBase()
        {
            this.WrappedObject = new Blank();
        }
        
        protected override void AppendContent(StringBuilder contentBuilder)
        {
            contentBuilder.Append(this.WrappedObject.ToHtml());
        }
    }
}