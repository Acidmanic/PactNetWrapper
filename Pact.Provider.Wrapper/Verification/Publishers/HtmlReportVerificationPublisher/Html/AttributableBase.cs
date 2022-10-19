using System.Collections.Generic;
using System.Text;

namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public abstract class AttributableBase : IHyperTextObject
    {
        
        private readonly Dictionary<string, string> _attributes = new Dictionary<string, string>();

        protected AttributableBase()
        {
        }

        public string ToHtml()
        {
            StringBuilder contentBuilder = new StringBuilder();

            contentBuilder.Append($"<{Tag()} ");
            
            foreach (var attribute in this._attributes)
            {
                contentBuilder
                    .Append(attribute.Key)
                    .Append("=\"")
                    .Append(attribute.Value)
                    .Append("\" ");
            }

            contentBuilder.Append(">");

            this.AppendContent(contentBuilder);
            
            contentBuilder.Append($"</{Tag()}>");

            return contentBuilder.ToString();
        }

        protected abstract void AppendContent(StringBuilder contentBuilder);
        
        public IHyperTextObject Attribute(string name, string value)
        {
            this._attributes.Add(name,value);

            return this;
        }

        protected abstract string Tag();
        

    }
}