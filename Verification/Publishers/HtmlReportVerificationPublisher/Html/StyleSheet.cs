using System.Collections.Generic;
using System.Text;

namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class StyleSheet:Dictionary<string,Style>, IHyperTextObject
    {
        public string ToHtml()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("<style>");
            
            foreach (var keyValue in this)
            {
                stringBuilder.Append(keyValue.Key)
                    .Append(keyValue.Value.ToHtml())
                    .Append("\n");
            }

            stringBuilder.Append("</style>");
            
            return stringBuilder.ToString();
        }

        public IHyperTextObject Attribute(string name, string value)
        {
            return this;
        }

        public StyleSheet Append(string name, Style style)
        {
            this.Add(name,style);

            return this;
        }
    }
}