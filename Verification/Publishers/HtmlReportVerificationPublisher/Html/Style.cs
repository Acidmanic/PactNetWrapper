using System.Collections.Generic;
using System.Text;

namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class Style:Dictionary<string,string>,IHyperTextObject
    {
        public string ToHtml()
        {
            StringBuilder contentBuilder = new StringBuilder();

            contentBuilder.Append("{");

            foreach (var keyValue in this)
            {
                contentBuilder
                    .Append(keyValue.Key)
                    .Append(":")
                    .Append(keyValue.Value)
                    .Append(";");
            }

            contentBuilder.Append("}");

            return contentBuilder.ToString();
        }

        public IHyperTextObject Attribute(string name, string value)
        {
            return this;
        }

        public Style Append(string key, string value)
        {
            this.Add(key,value);

            return this;
        }
    }
    
}