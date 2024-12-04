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
using WolfDen.Application.Requests.Queries.Attendence.SendWeeklyEmail;
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
                DateOnly weekEnd = DateOnly.FromDateTime(now).AddDays(-1);
                DateOnly weekStart = DateOnly.FromDateTime(now).AddDays(-5);
                string monday = weekStart.ToString();
                string friday = weekEnd.ToString();
                List<Employee>? managers = await _context.Employees
                    .Where(x => _context.Employees.Any(e => e.ManagerId == x.Id)) 
                    .ToListAsync();

                string subject = "Weekly Report";
                foreach (Employee employee in managers)
                {
                    List<ManagerWeeklyAttendanceDTO> managerWeeklyAttendanceDTOs = new List<ManagerWeeklyAttendanceDTO>();
                    List<Employee>? subOrdinates = await _context.Employees
                        .Where(e => e.ManagerId == employee.Id).ToListAsync();
                        foreach (Employee subEmployee in subOrdinates)
                        {

                            ManagerWeeklyAttendanceDTO managerWeeklyAttendanceDTO = new ManagerWeeklyAttendanceDTO();
                            WeeklySummaryQuery weeklySummary = new WeeklySummaryQuery();
                            weeklySummary.WeekStart = monday;
                            weeklySummary.WeekEnd = friday;
                            weeklySummary.EmployeeId = subEmployee.Id;
                            List<WeeklySummaryDTO> summary = await _mediator.Send(weeklySummary);
                            managerWeeklyAttendanceDTO.EmployeeName = subEmployee.FirstName + " " + subEmployee.LastName;
                            managerWeeklyAttendanceDTO.WeeklySummary = summary;
                            managerWeeklyAttendanceDTOs.Add(managerWeeklyAttendanceDTO);

                        }
                        IDocument document = _weeklyPdfService.CreateDocument(managerWeeklyAttendanceDTOs);
                        byte[] pdf = document.GeneratePdf();
                        string[] receiverEmails = { employee.Email };
                        List<string> managerEmails = await _emailFinder.FindManagerEmailsAsync(employee.ManagerId);
                        SendWeeklyMail(_senderEmail, _senderName, receiverEmails, pdf, subject, managerEmails.ToArray());  
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
                string pdfBase64 = Convert.ToBase64String(pdfAttachment);

                string htmlContent = "<h1>Weekly Attendance Report</h1><p>Please find the attached weekly attendance report in PDF format.</p>";

                var attachment = new SendSmtpEmailAttachment
                {
                    Name = "weeklyReport.pdf",
                    Content = pdfAttachment
                };
                List<SendSmtpEmailTo> toList = receiverEmails.Select(email => new SendSmtpEmailTo(email)).ToList();
                List<SendSmtpEmailCc> ccList = ccEmails?.Select(email => new SendSmtpEmailCc(email)).ToList() ?? new List<SendSmtpEmailCc>();
                var sendSmtpEmail = new SendSmtpEmail
                {
                    Sender = sender,
                    To = toList,
                    Cc = ccList,
                    HtmlContent=htmlContent,
                    Subject = subject
                }; sendSmtpEmail.Attachment = new List<SendSmtpEmailAttachment> { attachment };
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