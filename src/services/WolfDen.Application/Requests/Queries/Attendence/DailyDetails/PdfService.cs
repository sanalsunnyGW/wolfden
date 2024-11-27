using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WolfDen.Application.Requests.DTOs.Attendence;
using WolfDen.Domain.Enums;

namespace WolfDen.Application.Requests.Queries.Attendence.DailyDetails
{
    public class PdfService
    {
        public IDocument CreateDocument(DailyAttendanceDTO dailyStatusDTO)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {

                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12));
                        page.Header()
                            .Text("Monthly Attendance Report")
                            .AlignCenter()
                            .SemiBold().FontSize(30).FontColor(Colors.Black);

                        page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                         {
                             column.Item().PaddingLeft(2, Unit.Centimetre).Column(col =>
                              {

                                  col.Item().Row(row =>
                                  {
                                      row.RelativeItem().Padding(5).AlignLeft().Text($"Arrival Time: {dailyStatusDTO.ArrivalTime}")
                                          .SemiBold().FontSize(14).FontColor(Colors.Black);

                                  });
                                  col.Item().Row(row =>
                                  {
                                      row.RelativeItem().Padding(5).AlignLeft().Text($"Departure Time: {dailyStatusDTO.DepartureTime}")
                                         .SemiBold().FontSize(14).FontColor(Colors.Black);
                                  });
                                  col.Item().Row(row =>
                                  {
                                      row.RelativeItem().Padding(5).AlignLeft().Text($"Inside Duration: {dailyStatusDTO.InsideHours} minutes")
                                          .SemiBold().FontSize(14).FontColor(Colors.Black);

                                  });
                                  string status = "";
                                  switch (dailyStatusDTO.AttendanceStatusId)
                                  {
                                      case AttendanceStatus.Present:
                                          status = "Present";
                                          break;
                                      case AttendanceStatus.Absent:
                                          status = "Absent";
                                          break;
                                      case AttendanceStatus.IncompleteShift:
                                          status = "Incomplete Shift";
                                          break;
                                      case AttendanceStatus.RestrictedHoliday:
                                          status = "Restricted Holiday";
                                          break;
                                      case AttendanceStatus.NormalHoliday:
                                          status = "Normal Holiday";
                                          break;
                                      case AttendanceStatus.WFH:
                                          status = "Work FRom Home";
                                          break;
                                      default:
                                          status = "leave";
                                          break;
                                  }
                                  col.Item().Row(row =>
                                  {
                                      row.RelativeItem().Padding(5).AlignLeft().Text($"Status: {status}")
                                          .SemiBold().FontSize(14).FontColor(Colors.Black);
                                  });
                                  col.Item().Row(row =>
                                  {
                                      row.RelativeItem().Padding(5).AlignLeft().Text($"Outside Duration: {dailyStatusDTO.OutsideHours} minutes")
                                          .SemiBold().FontSize(14).FontColor(Colors.Black);
                                  });
                                  col.Item().Row(row =>
                                  {
                                      row.RelativeItem().Padding(5).AlignLeft().Text($"Missed Punch: {dailyStatusDTO.MissedPunch}")
                                          .SemiBold().FontSize(14).FontColor(Colors.Black);
                                  });
                              });

                             column.Item().PaddingVertical(1, Unit.Centimetre).Table(table =>
                             {
                                 table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });
                                 table.Header(header =>
                                {
                                    header.Cell().Border(1).BorderColor(Colors.Black)
                                          .Background(Colors.Grey.Lighten3)
                                          .Padding(10).AlignCenter().Text("Time").Bold().FontSize(12);
                                    header.Cell().Border(1).BorderColor(Colors.Black)
                                          .Background(Colors.Grey.Lighten3)
                                          .Padding(10).AlignCenter().Text("Device Name").Bold().FontSize(12);
                                    header.Cell().Border(1).BorderColor(Colors.Black)
                                          .Background(Colors.Grey.Lighten3)
                                          .Padding(10).AlignCenter().Text("Direction").Bold().FontSize(12);
                                });
                                 foreach (var attendenceLog in dailyStatusDTO.DailyLog)
                                 {
                                     string direction = "in";
                                     table.Cell().Border(1).Padding(10).Text(attendenceLog.Time.ToString("HH:mm")).FontSize(12);


                                     table.Cell().Border(1).Padding(10).Text(attendenceLog.DeviceName).FontSize(12);

                                     if (attendenceLog.Direction == 0)
                                     {
                                         direction = "out";
                                     }
                                     table.Cell().Border(1).Padding(10).Text(direction).FontSize(12);

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
                });
            });
        }
    }
}








