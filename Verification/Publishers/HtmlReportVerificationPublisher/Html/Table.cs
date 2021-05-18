using System.Collections.Generic;
using System.Text;

namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class Table : AttributableBase
    {
        public List<TableRow> Rows { get; } = new List<TableRow>();


        protected override void AppendContent(StringBuilder contentBuilder)
        {
            this.Rows.ForEach(r => contentBuilder.Append(r.ToHtml()));
        }
        
        protected override string Tag()
        {
            return "table";
        }
    }
}