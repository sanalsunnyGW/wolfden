using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WolfDen.Application.Requests.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Attendence.DailyStatus
{
    public class PdfService
    {
        public IDocument CreateDocument(DailyStatusDTO dailyStatusDTO)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text("Daily Attendence Report")
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                          
                           
                           column.Item().Text($"Arrival Time: {dailyStatusDTO.ArrivalTime}")
                                .SemiBold().FontSize(16).FontColor(Colors.Black);
                            column.Item().Text($"Departure Time: {dailyStatusDTO.DepartureTime}")
                                .SemiBold().FontSize(16).FontColor(Colors.Black);
                            column.Item().Text($"Inside Hours: {dailyStatusDTO.InsideHours}")
                                .SemiBold().FontSize(16).FontColor(Colors.Black);
                            column.Item().Text($"Status: {dailyStatusDTO.Status}")
                                .SemiBold().FontSize(16).FontColor(Colors.Black);
                            column.Item().Text($"Outside Hours: {dailyStatusDTO.OutsideHours}")
                                .SemiBold().FontSize(16).FontColor(Colors.Black);
                            column.Item().Text($"Missed Punch: {dailyStatusDTO.MissedPunch}")
                               .SemiBold().FontSize(16).FontColor(Colors.Black);
                            
                            

                            column.Item().Table(table =>
                            {

                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(70);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });




                                table.Header(header =>
                                {
                                    header.Cell().Text("Time").Bold().FontSize(12);
                                    header.Cell().Text("DeviceName").Bold().FontSize(12);
                                    header.Cell().Text("Direction").Bold().FontSize(12);
                                });


                                foreach (var attendenceLog in dailyStatusDTO.DailyLog)
                                {
                                    string direction = "in";
                                    table.Cell().Text(attendenceLog.Time.ToString("HH:mm"));
                                    table.Cell().Text(attendenceLog.DeviceName);
                                    if(attendenceLog.Direction==0)
                                    {
                                        direction = "out";
                                    }
                                    table.Cell().Text(direction);
                                   
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
