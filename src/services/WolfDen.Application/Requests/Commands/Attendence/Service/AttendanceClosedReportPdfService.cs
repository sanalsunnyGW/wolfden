using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Domain.Entity;

namespace WolfDen.Application.Requests.Commands.Attendence.Service
{
    public class AttendanceClosedReportPdfService
    {
        public IDocument CreateDocument(List<AllEmployeesMonthlyReportDTO> allEmployeesMonthlyReports)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));
                    page.Header()
                        .Text("Attendance Report")
                        .AlignCenter()
                        .SemiBold().FontSize(30).FontColor(Colors.Black);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            
                            column.Item().PaddingLeft(1, Unit.Centimetre).PaddingBottom(1,Unit.Centimetre).Column(col =>
                            {
                                col.Item().Row(row =>
                                {
                                    row.RelativeItem().Padding(1).AlignLeft()
                                    .Text($"Date: {allEmployeesMonthlyReports[0].RangeStart} to {allEmployeesMonthlyReports[0].RangeEnd}")
                                    .SemiBold().FontSize(14).FontColor(Colors.Black);
                                });
                            });
                                foreach (AllEmployeesMonthlyReportDTO employee in allEmployeesMonthlyReports)
                            {
                                column.Item().PaddingLeft(1, Unit.Centimetre).Column(col =>
                                {
 
                                    col.Item().Row(row =>
                                    {
                                        row.RelativeItem().Padding(1).AlignLeft()
                                        .Text($"Name: {employee.EmployeeName}")
                                        .SemiBold().FontSize(14).FontColor(Colors.Black);
                                    });
                                    if(employee.NoOfAbsentDays>0)
                                    {
                                        col.Item().Row(row =>
                                        {
                                            row.RelativeItem().Padding(1).AlignLeft()
                                            .Text($"Absent Days: {employee.AbsentDays}")
                                            .SemiBold().FontSize(14).FontColor(Colors.Black);
                                        });
                                    }
                                    if (employee.NofIncompleteShiftDays > 0)
                                    {
                                        col.Item().Row(row =>
                                        {
                                            row.RelativeItem().Padding(1).AlignLeft()
                                            .Text($"Incomplete Shift Days: {employee.IncompleteShiftDays}")
                                            .SemiBold().FontSize(14).FontColor(Colors.Black);
                                        });
                                    }
                                    if (employee.HalfDays > 0)
                                    {
                                        col.Item().Row(row =>
                                        {
                                            row.RelativeItem().Padding(1).AlignLeft()
                                            .Text($"Half Day Leave Days: {employee.HalfDayLeaves}")
                                            .SemiBold().FontSize(14).FontColor(Colors.Black);
                                        });
                                    }


                                    col.Item().PaddingVertical(1, Unit.Centimetre).Table(table =>
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
                                                .Padding(3).AlignCenter().Text("No of Absent Days")
                                                .Bold().FontSize(12);
                                            
                                            header.Cell().Border(1).BorderColor(Colors.Black)
                                                .Background(Colors.Grey.Lighten3)
                                                .Padding(3).AlignCenter().Text("No of Incomplete Shift Days")
                                                .Bold().FontSize(12);
                                            
                                            header.Cell().Border(1).BorderColor(Colors.Black)
                                                .Background(Colors.Grey.Lighten3)
                                                .Padding(3).AlignCenter().Text("No of Half Day Leaves")
                                                .Bold().FontSize(12);
                                           
                                        });
                                       
                                        table.Cell().Border(1).Padding(3)
                                             .Text(employee.NoOfAbsentDays.ToString()).FontSize(12);
                                           
                                        table.Cell().Border(1).Padding(3)
                                             .Text(employee.NofIncompleteShiftDays.ToString()).FontSize(12);
                                            
                                        table.Cell().Border(1).Padding(3)
                                             .Text(employee.HalfDays.ToString()).FontSize(12);
                                            
                                    });
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
