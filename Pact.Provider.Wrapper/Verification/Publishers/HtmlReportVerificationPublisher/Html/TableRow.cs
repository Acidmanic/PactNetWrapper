using System.Collections.Generic;
using System.Text;

namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html
{
    public class TableRow : AttributableBase
    {
        public List<TableCell> Cells { get; } = new List<TableCell>();

        protected override void AppendContent(StringBuilder contentBuilder)
        {
            this.Cells.ForEach(c => contentBuilder.Append(c.ToHtml()));
        }

        protected override string Tag()
        {
            return "tr";
        }
    }
}