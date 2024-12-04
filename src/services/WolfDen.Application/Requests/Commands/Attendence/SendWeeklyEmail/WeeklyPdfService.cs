using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WolfDen.Application.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Attendence.SendWeeklyEmail
{
    public class WeeklyPdfService
    {
        public IDocument CreateDocument(List<ManagerWeeklyAttendanceDTO> managerWeeklyAttendanceDTOs)
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
                        .Text("Weekly Attendance Report")
                        .AlignCenter()
                        .SemiBold().FontSize(30).FontColor(Colors.Black);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            foreach (var employee in managerWeeklyAttendanceDTOs)
                            {
                                column.Item().PaddingLeft(2, Unit.Centimetre).Column(col =>
                                {
                                    
                                    col.Item().Row(row =>
                                    {
                                        row.RelativeItem().Padding(5).AlignLeft()
                                        .Text($"Name: {employee.EmployeeName}")
                                        .SemiBold().FontSize(14).FontColor(Colors.Black);
                                    });

                                  
                                    col.Item().PaddingVertical(1, Unit.Centimetre).Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                        });

                                        table.Header(header =>
                                        {
                                            header.Cell().Border(1).BorderColor(Colors.Black)
                                                .Background(Colors.Grey.Lighten3)
                                                .Padding(3).AlignCenter().Text("Date")
                                                .Bold().FontSize(12);
                                            header.Cell().Border(1).BorderColor(Colors.Black)
                                                .Background(Colors.Grey.Lighten3)
                                                .Padding(3).AlignCenter().Text("Arrival Time")
                                                .Bold().FontSize(12);
                                            header.Cell().Border(1).BorderColor(Colors.Black)
                                                .Background(Colors.Grey.Lighten3)
                                                .Padding(3).AlignCenter().Text("Departure Time")
                                                .Bold().FontSize(12);
                                            header.Cell().Border(1).BorderColor(Colors.Black)
                                                .Background(Colors.Grey.Lighten3)
                                                .Padding(3).AlignCenter().Text("Missed Punch")
                                                .Bold().FontSize(12);
                                            header.Cell().Border(1).BorderColor(Colors.Black)
                                                .Background(Colors.Grey.Lighten3)
                                                .Padding(3).AlignCenter().Text("Status")
                                                .Bold().FontSize(12);
                                            header.Cell().Border(1).BorderColor(Colors.Black)
                                                .Background(Colors.Grey.Lighten3)
                                                .Padding(3).AlignCenter().Text("Inside Duration")
                                                .Bold().FontSize(12);
                                        });

                                        foreach (var weeklySummary in employee.WeeklySummary)
                                        {
                                            table.Cell().Border(1).Padding(3)
                                                .Text(weeklySummary.Date).FontSize(12);
                                            table.Cell().Border(1).Padding(3)
                                                .Text(weeklySummary.ArrivalTime).FontSize(12);
                                            table.Cell().Border(1).Padding(3)
                                                .Text(weeklySummary.DepartureTime).FontSize(12);
                                            table.Cell().Border(1).Padding(3)
                                                .Text(weeklySummary.MissedPunch).FontSize(12);
                                            table.Cell().Border(1).Padding(3)
                                                .Text(weeklySummary.AttendanceStatusId).FontSize(12);
                                            if(weeklySummary.InsideDuration!=null)
                                            {
                                                int? hours = weeklySummary.InsideDuration / 60;
                                                int? minutes = weeklySummary.InsideDuration % 60;

                                                table.Cell().Border(1).Padding(3)
                                                    .Text($"{hours}h {minutes}m").FontSize(12);
                                            }
                                            else
                                            {
                                                table.Cell().Border(1).Padding(3)
                                                   .Text($"-").FontSize(12);
                                            }
                                           
                                        }
                                    });
                                    col.Item().PageBreak();
                                });
                            }
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
