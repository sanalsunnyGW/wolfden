using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;
using WolfDen.Application.Helpers;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Attendence.Email
{
    public class SendEmailCommandHandler: IRequestHandler<SendEmailCommand, string>
    {
        private readonly WolfDenContext _context;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly ManagerEmailFinder _emailFinder;
        public SendEmailCommandHandler(WolfDenContext context, IConfiguration configuration,ManagerEmailFinder emailFinder)
        {
            _context = context;
            _senderEmail = configuration["BrevoApi:SenderEmail"];
            _senderName = configuration["BrevoApi:SenderName"];
            _emailFinder = emailFinder;
        }
        public async Task<string> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .Where(e => e.Id == request.EmployeeId)
                .Select(e => new { e.Email, e.FirstName, e.LastName, e.ManagerId })
                .FirstOrDefaultAsync(cancellationToken);
       
            string[] receiverEmails = { employee.Email };
            List<string> managerEmails = await _emailFinder.FindManagerEmailsAsync(employee.ManagerId, cancellationToken);

            string subject = "Attendance Low Warning";
            string message = $"{employee.FirstName} {employee.LastName}'s working hours are below the required threshold.";

            SendMail(_senderEmail, _senderName, receiverEmails, message, subject, managerEmails.ToArray());
            return "success";
        }

        private void SendMail(string senderEmail, string senderName, string[] receiverEmails, string message, string subject, string[] ccEmails = null)
        {
           
                TransactionalEmailsApi apiInstance = new TransactionalEmailsApi();

                SendSmtpEmailSender sender = new SendSmtpEmailSender
                {
                    Email = senderEmail,
                    Name = senderName
                };

                List<SendSmtpEmailTo> toList = receiverEmails.Select(email => new SendSmtpEmailTo(email)).ToList();
                List<SendSmtpEmailCc> ccList = ccEmails?.Select(email => new SendSmtpEmailCc(email)).ToList() ?? new List<SendSmtpEmailCc>();

                SendSmtpEmail sendSmtpEmail = new SendSmtpEmail(sender, toList, null, ccList, null, message, subject);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);

                Console.WriteLine($"Email sent successfully: {result.ToJson()}");
        }
    }
}