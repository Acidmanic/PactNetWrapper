using System.Collections.Generic;
using System.Text;

namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class Body:IHyperTextObject
    {
        
        public List<IHyperTextObject> Content { get; } = new List<IHyperTextObject>();
        private readonly Dictionary<string, string> _attributes = new Dictionary<string, string>();
        
        public string ToHtml()
        {
            StringBuilder contentBuilder = new StringBuilder();

            contentBuilder.Append("<body ");

            foreach (var attribute in this._attributes)
            {
                contentBuilder
                    .Append(attribute.Key)
                    .Append("\"")
                    .Append(attribute.Value)
                    .Append("\" ");
            }

            contentBuilder.Append(">");
            
            this.Content.ForEach(c => contentBuilder.Append(c.ToHtml()));

            contentBuilder.Append("</body>");
            
            return contentBuilder.ToString();
        }

        public IHyperTextObject Attribute(string name, string value)
        {
            this._attributes.Add(name,value);

            return this;
        }
    }
}