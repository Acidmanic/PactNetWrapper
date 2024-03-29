using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html;


namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher
{
    public class SimpleTableReport : Html.Html
    {
        private readonly IPublicationTagger _tagger;

        public SimpleTableReport(List<VerificationRecord> verificationRecords)
            : this(verificationRecords, new ColumnDelimitedPublicationTagger())
        {
        }

        public SimpleTableReport(List<VerificationRecord> verificationRecords, IPublicationTagger tagger)
        {
            _tagger = tagger;

            Head = new Head {Title = "Pact Verification Results"};

            AddStyles(Head);

            var divContainer = new Div();

            Body = new Body();

            Body.Content.Add(divContainer);

            divContainer.Childs.Add(new Heading(1, new Text("Test Results.")));

            divContainer.Childs.Add(new Paragraph(new Text("Following Table shows the result os " +
                                                           "running pact tests on backend application. each table row, " +
                                                           "represents the result of one api endpoint. Since each endpoint" +
                                                           " might have several interactions verified against, the end point " +
                                                           "verification status, shows represents the success of all " +
                                                           "interactions or failure of more than zero interactions.")));

            var table = new Table {Rows = {TableHeader()}};

            verificationRecords.ForEach(r => table.Rows.Add(Row(r)));

            divContainer.Childs.Add(new Paragraph(table));

            divContainer.Attribute("class", "div-container");
        }

        private void AddStyles(Head head)
        {
            head.StyleSheets.Add(
                new StyleSheet()
                    .Append(
                        "td", new Style()
                            .Append("padding", "15px")
                            .Append("text-align", "center")
                    ).Append(
                        ".table-header", new Style()
                            .Append("font-size", "1.1em")
                            .Append("background-color", "darkgray")
                            .Append("color", "white")
                    ).Append(
                        ".table-cell", new Style()
                            .Append("font-size", "0.94em")
                            .Append("background-color", "white")
                            .Append("color", "black")
                    ).Append(
                        ".table-cell-pass", new Style()
                            .Append("background-color", "#f0f4c3ff")
                            .Append("color", "#388e3cff")
                    ).Append(
                        ".table-cell-fail", new Style()
                            .Append("background-color", "#d32f2fff")
                            .Append("color", "white")
                    ).Append(
                        "table", new Style()
                            .Append("border", "solid black 1px")
                    ).Append(
                        "div.div-container", new Style()
                            .Append("width", "70%")
                            .Append("margin", "auto")
                            .Append("margin-top", "120px")
                    ).Append(
                        ".details-paragraph", new Style()
                            .Append("font-size", "0.8em")
                            .Append("font-weight", "normal")
                            .Append("text-align", "left")
                    ).Append(
                        ".table-cell-state", new Style()
                            .Append("font-size", "0.7em")
                            .Append("text-align", "left")
                            .Append("background-color", "white")
                            .Append("color", "black")
                    ).Append(".method",new Style()
                        .Append("font-style","bold")
                        .Append("border-radius","32px")
                        .Append("padding","8px")
                        .Append("font-size","0.9em")
                    ).Append(".method-get", new Style()
                        .Append("background-color","#7ec552")
                    ).Append(".method-post", new Style()
                        .Append("background-color","#3053a3")
                        .Append("color","white")
                    ).Append(".method-put", new Style()
                        .Append("background-color","#958bad")
                    ).Append(".method-delete", new Style()
                        .Append("background-color","#bf4242")
                        .Append("color","white")
                    ).Append("body",new Style()
                        .Append("font-family","system-ui")
                    )
            );
        }

        private TableRow Row(VerificationRecord record)
        {
            var tableRow = new TableRow();
            
            tableRow.Cells.Add(new TableCell(
                new Italic(new Text(record.Interaction.ProviderState))
            ).Attribute("class", "table-cell-state") as TableCell);

            tableRow.Cells.Add(new TableCell(
                new Italic(new Text($"{record.Interaction.ConsumerName}::{record.Interaction.ProviderName}"))
            ).Attribute("class", "table-cell") as TableCell);

            var method = record.Interaction.RequestMethod.Method;


            var methodSpan = new Span();
            methodSpan.Childs.Add(new Text(method.ToUpper()));
            methodSpan.Attribute("class", "method method-" + method.ToLower());
            
            tableRow.Cells.Add(new TableCell(methodSpan)
                .Attribute("class", "table-cell") as TableCell);

            tableRow.Cells.Add(new TableCell(
                new Italic(new Text(record.Interaction.RequestPath))
            ).Attribute("class", "table-cell") as TableCell);

            var status = record.Success ? "PASS" : "FAIL";

            tableRow.Cells.Add(new TableCell(
                new Bold(new Text(status))
            ).Attribute("class", "table-cell table-cell-" + status.ToLower()) as TableCell);

            var details = record.Logs;

            if (string.IsNullOrEmpty(details.Trim()))
            {
                details = record.Success? "&#x2713;" : "&#x58;";
                
                tableRow.Cells.Add(new TableCell(
                    new Bold(new Text(details))
                ).Attribute("class", "table-cell table-cell-" + status.ToLower()) as TableCell);
                
            }
            else
            {
                details = ParagraphEncode(details);
                
                tableRow.Cells.Add(new TableCell(new Text(details))
                    .Attribute("class", "table-cell details-paragraph") as TableCell);
            }

            return tableRow;
        }

        private string ParagraphEncode(string text)
        {
            text = text.Replace("\r", "\n");

            while (text.Contains("\n\n"))
            {
                text = text.Replace("\n\n", "\n");
            }

            text = HttpUtility.HtmlEncode(text);

            text = text.Replace("\n", "<br>");

            return text;
        }

        private TableRow TableHeader()
        {
            var tableRow = new TableRow();
            
            tableRow.Cells.Add(new TableCell(
                new Bold(new Text("Provider State"))
            ).Attribute("class", "table-header") as TableCell);

            tableRow.Cells.Add(new TableCell(
                new Bold(new Text("Pact"))
            ).Attribute("class", "table-header") as TableCell);
            
            tableRow.Cells.Add(new TableCell(
                new Bold(new Text("Method"))
            ).Attribute("class", "table-header") as TableCell);

            tableRow.Cells.Add(new TableCell(
                new Bold(new Text("Api Endpoint"))
            ).Attribute("class", "table-header") as TableCell);

            tableRow.Cells.Add(new TableCell(
                new Bold(new Text("Status"))
            ).Attribute("class", "table-header") as TableCell);

            tableRow.Cells.Add(new TableCell(
                new Bold(new Text("Details"))
            ).Attribute("class", "table-header") as TableCell);
            return tableRow;
        }
    }
}