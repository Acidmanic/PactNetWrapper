using System.Collections.Generic;
using System.Text;

namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public abstract class ContainerBase : AttributableBase
    {
        public List<IHyperTextObject> Childs { get; } = new List<IHyperTextObject>();

        protected ContainerBase()
        {
        }

        protected ContainerBase(params IHyperTextObject[] childs)
        {
            this.Childs.AddRange(childs);
        }
        
        protected override void AppendContent(StringBuilder contentBuilder)
        {
            this.Childs.ForEach(child => contentBuilder.Append(child.ToHtml()));
        }

    }
}