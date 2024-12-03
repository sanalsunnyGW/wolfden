using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using WolfDen.Application.Helpers;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;


namespace WolfDen.Application.Requests.Commands.Attendence.Email
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, bool>
    {
        private readonly WolfDenContext _context;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly ManagerEmailFinder _emailFinder;
        private readonly string _apiKey;
        public SendEmailCommandHandler(WolfDenContext context, IConfiguration configuration, ManagerEmailFinder emailFinder)
        {
            _context = context;
            _senderEmail = configuration["BrevoApi:SenderEmail"];
            _senderName = configuration["BrevoApi:SenderName"];
            _emailFinder = emailFinder;
            _apiKey = configuration["BrevoApi:ApiKey"];
        }
        public async Task<bool> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            Employee? employee = await _context.Employees
              .Where(e => e.Id == request.EmployeeId).FirstOrDefaultAsync(cancellationToken);
            string[] receiverEmails = { employee.Email };
            List<string> managerEmails = await _emailFinder.FindManagerEmailsAsync(employee.ManagerId, cancellationToken);
            string subject = request.Subject;
            string message = request.Message;
            bool status = SendMail(_senderEmail, _senderName, receiverEmails, message, subject, managerEmails.ToArray());
            if (status)
            {
                DailyAttendence? attendence = await _context.DailyAttendence
               .Where(a => a.Date == DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1) && a.EmployeeId == employee.Id).FirstOrDefaultAsync(cancellationToken);
                attendence.Update();
                _context.Update(attendence);
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }
            return false;
        }
        private bool SendMail(string senderEmail, string senderName, string[] receiverEmails, string message, string subject, string[] ccEmails = null)
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
                List<SendSmtpEmailTo> toList = receiverEmails.Select(email => new SendSmtpEmailTo(email)).ToList();
                List<SendSmtpEmailCc> ccList = ccEmails?.Select(email => new SendSmtpEmailCc(email)).ToList() ?? new List<SendSmtpEmailCc>();
                SendSmtpEmail sendSmtpEmail = new SendSmtpEmail(sender, toList, null, ccList, null, message, subject);
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