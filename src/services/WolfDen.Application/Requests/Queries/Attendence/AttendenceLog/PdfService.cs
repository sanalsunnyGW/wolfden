using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WolfDen.Application.Requests.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Attendence.AttendenceLog
{
    public class PdfService
    {
        public IDocument CreateDocument(List<AttendenceLogDTO> AttendenceLogDTOs)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text("Daily Attendence Report")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, QuestPDF.Infrastructure.Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Item().Table(table =>
                            {

                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(2);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });


                                table.Header(header =>
                                {
                                    header.Cell().Text("Time").Bold().FontSize(12);
                                    header.Cell().Text("DeviceName").Bold().FontSize(12);
                                    header.Cell().Text("Direction").Bold().FontSize(12);
                                });


                                foreach (var attendenceLog in AttendenceLogDTOs)
                                {

                                    table.Cell().Text(attendenceLog.Time.ToString("HH:mm"));
                                    table.Cell().Text(attendenceLog.DeviceName);
                                    table.Cell().Text(attendenceLog.Direction);
                                }
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });
        }
    }
}
