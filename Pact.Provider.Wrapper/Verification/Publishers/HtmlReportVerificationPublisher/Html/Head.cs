using System.Collections.Generic;
using System.Text;

namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class Head:IHyperTextObject
    {
        private readonly List<StyleSheet> _styleSheets = new List<StyleSheet>();
        public string Title { get; set; }

        public List<StyleSheet> StyleSheets => _styleSheets;

        public string ToHtml()
        {
            StringBuilder sheets = new StringBuilder();

            foreach (var styleSheet in this.StyleSheets)
            {
                sheets.Append(styleSheet.ToHtml());
            }
            
            return $"<head><title>{this.Title}</title>{sheets}</head>";
        }

        public IHyperTextObject Attribute(string name, string value)
        {
            return this;
        }
    }
}