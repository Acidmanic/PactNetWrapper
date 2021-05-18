using System.Collections.Generic;
using System.Linq;
using Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher.Html;


namespace Pact.Provider.Wrapper.Verification.Publishers.HtmlReportVerificationPublisher
{
    public class SimpleTableReport : Html.Html
    {

        private readonly IInteractionTagger _tagger ;

        public SimpleTableReport(List<VerificationRecord> verificationRecords)
            : this(verificationRecords, new ColumnDelimitedInteractionTagger())
        {
        }

        public SimpleTableReport(List<VerificationRecord> verificationRecords,IInteractionTagger tagger)
        {
            _tagger = tagger;
            
            Head = new Head {Title = "Pact Verification Results"};

            AddStyles(Head);
            
            var divContainer = new Div();
            
            Body = new Body();
            
            Body.Content.Add(divContainer);

            divContainer.Childs.Add(new Heading(1, new Text("Test Results.")));
            
            divContainer.Childs.Add( new Paragraph(new Text("Following Table shows the result os " +
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

                    )
            );
        }

        private TableRow Row(VerificationRecord record)
        {
            var tableRow = new TableRow();

            tableRow.Cells.Add(new TableCell());

            string tag = _tagger.Tag(record.Interaction);

            tableRow.Cells.Add(new TableCell(
                new Italic(new Text(tag))
            ).Attribute("class", "table-cell") as TableCell);
            
            tableRow.Cells.Add(new TableCell(
                new Italic(new Text($"{record.Interaction.ConsumerName}::{record.Interaction.ProviderName}" ))
            ).Attribute("class", "table-cell") as TableCell);
            
            tableRow.Cells.Add(new TableCell(
                new Italic(new Text(record.Interaction.RequestPath))
            ).Attribute("class", "table-cell") as TableCell);

            var status = record.Success ? "PASS" : "FAIL";

            tableRow.Cells.Add(new TableCell(
                new Bold(new Text(status))
            ).Attribute("class", "table-cell table-cell-" + status.ToLower()) as TableCell);

            return tableRow;
        }

        private TableRow TableHeader()
        {
            var tableRow = new TableRow();

            tableRow.Cells.Add(new TableCell());

            tableRow.Cells.Add(new TableCell(
                new Bold(new Text("Tag"))
            ).Attribute("class", "table-header") as TableCell);
            
            tableRow.Cells.Add(new TableCell(
                new Bold(new Text("Pact"))
            ).Attribute("class", "table-header") as TableCell);

            tableRow.Cells.Add(new TableCell(
                new Bold(new Text("Api Endpoint"))
            ).Attribute("class", "table-header") as TableCell);
            
            tableRow.Cells.Add(new TableCell(
                new Bold(new Text("Status"))
            ).Attribute("class", "table-header") as TableCell);

            return tableRow;
        }
       
    }
}