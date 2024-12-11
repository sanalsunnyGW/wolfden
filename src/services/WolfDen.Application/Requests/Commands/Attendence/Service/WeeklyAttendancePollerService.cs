using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Application.Helpers;
using WolfDen.Application.Requests.Queries.Attendence.WeeklySummary;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;



namespace WolfDen.Application.Requests.Commands.Attendence.Service
{
    public class WeeklyAttendancePollerService
    {
        private readonly WolfDenContext _context;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly ManagerEmailFinder _emailFinder;
        private readonly string _apiKey;
        private readonly IMediator _mediator;
        private readonly WeeklyPdfService _weeklyPdfService;
        private readonly IServiceScopeFactory _serviceScopeFactory;
       

        public WeeklyAttendancePollerService(WolfDenContext context, IConfiguration configuration, ManagerEmailFinder emailFinder, IServiceScopeFactory serviceScopeFactory,IMediator mediator,WeeklyPdfService weeklyPdfService)
        {
            _context = context;
            _senderEmail = configuration["BrevoApi:SenderEmail"];
            _senderName = configuration["BrevoApi:SenderName"];
            _emailFinder = emailFinder;
            _apiKey = configuration["BrevoApi:ApiKey"];
            _serviceScopeFactory = serviceScopeFactory;
            _mediator = mediator;
            _weeklyPdfService = weeklyPdfService;
        }
        public async System.Threading.Tasks.Task WeeklyEmail()
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                WolfDenContext _context = scope.ServiceProvider.GetRequiredService<WolfDenContext>();
                DateTime now = DateTime.Now;
                int currentDayOfWeek = (int)now.DayOfWeek;
                DateTime weekStart = now.AddDays(-currentDayOfWeek + (int)DayOfWeek.Monday);
                DateOnly weekStartDateOnly = DateOnly.FromDateTime(weekStart);
                string friday = "";
                if (now.Day != 6)
                {
                    DateOnly weekEndDateOnly = DateOnly.FromDateTime(now);
                    friday = weekEndDateOnly.ToString("yyyy-MM-dd");
                }
                else
                {
                    DateOnly weekEndDateOnly = DateOnly.FromDateTime(now).AddDays(-1);
                    friday = weekEndDateOnly.ToString("yyyy-MM-dd");

                }

                string monday = weekStartDateOnly.ToString("yyyy-MM-dd");
                

                string subject = "Weekly Attendance Report";

                
                var managerData = await _context.Employees
                    .Where(m => _context.Employees.Any(e => e.ManagerId == m.Id))
                    .Select(m => new
                    {
                        Manager = m,
                        Subordinates = _context.Employees.Where(e => e.ManagerId == m.Id)
                            .Select(e => new
                            {
                                e.Id,
                                e.FirstName,
                                e.LastName
                            }).ToList()
                    }).ToListAsync();

                foreach (var data in managerData)
                {
                    try
                    {
                        Employee manager = data.Manager;
                        var subordinates = data.Subordinates;

                        List<ManagerWeeklyAttendanceDTO> managerWeeklyAttendanceDTOs = new List<ManagerWeeklyAttendanceDTO>();

                        foreach (var sub in subordinates)
                        {
                            WeeklySummaryQuery weeklySummaryQuery = new WeeklySummaryQuery
                            {
                                WeekStart = monday,
                                WeekEnd = friday,
                                EmployeeId = sub.Id
                            };

                            List<WeeklySummaryDTO> summary = await _mediator.Send(weeklySummaryQuery);

                            managerWeeklyAttendanceDTOs.Add(new ManagerWeeklyAttendanceDTO
                            {
                                EmployeeName = $"{sub.FirstName} {sub.LastName}",
                                WeeklySummary = summary
                            });
                        }

                        IDocument document = _weeklyPdfService.CreateDocument(managerWeeklyAttendanceDTOs);
                        byte[] pdf = document.GeneratePdf();

                       
                        string?[] receiverEmails = [manager.Email];
                        List<string> managerEmails = await _emailFinder.FindManagerEmailsAsync(manager.ManagerId);

                       
                        SendWeeklyMail(_senderEmail, _senderName, receiverEmails, pdf, subject, managerEmails.ToArray());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing manager {data.Manager.Id}: {ex.Message}");
                    }
                }
            }
        }
            private bool SendWeeklyMail(string senderEmail, string senderName, string[] receiverEmails, byte[] pdfAttachment, string subject, string[] ccEmails = null)
             {

            try
            {
                Configuration configuration = new Configuration();
                configuration.AddApiKey("api-key", _apiKey);

                TransactionalEmailsApi apiInstance = new TransactionalEmailsApi(configuration);
                SendSmtpEmailSender sender = new SendSmtpEmailSender
                {
                    Email = senderEmail,
                    Name = senderName
                };
               
                string htmlContent = "<h1>Weekly Attendance Report</h1><p>Please find the attached weekly attendance report in PDF format.</p>";

                SendSmtpEmailAttachment attachment = new SendSmtpEmailAttachment
                {
                    Name = "weeklyReport.pdf",
                    Content = pdfAttachment
                };
                List<SendSmtpEmailTo> toList = receiverEmails.Select(email => new SendSmtpEmailTo(email)).ToList();
                List<SendSmtpEmailCc>? ccList = ccEmails != null && ccEmails.Length > 0
                            ? ccEmails.Select(email => new SendSmtpEmailCc(email)).ToList()
                            : null;

                SendSmtpEmail sendSmtpEmail = new SendSmtpEmail
                {
                    Sender = sender,
                    To = toList,
                    HtmlContent=htmlContent,
                    Subject = subject
                }; 
                sendSmtpEmail.Attachment = new List<SendSmtpEmailAttachment> { attachment };
                if (ccList != null && ccList.Any())
                {
                    sendSmtpEmail.Cc = ccList;
                }
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}