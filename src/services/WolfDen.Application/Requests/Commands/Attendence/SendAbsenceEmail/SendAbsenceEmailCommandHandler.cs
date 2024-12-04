using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Attendence.SendAbsenceEmail
{
    public class SendAbsenceEmailCommandHandler : IRequestHandler<SendAbsenceEmailCommand, bool>
    {
        private readonly WolfDenContext _context;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly string _apiKey;
        public SendAbsenceEmailCommandHandler(WolfDenContext context, IConfiguration configuration)
        {
            _context = context;
            _senderEmail = configuration["BrevoApi:SenderEmail"];
            _senderName = configuration["BrevoApi:SenderName"];
            _apiKey = configuration["BrevoApi:ApiKey"];
        }

        public async Task<bool> Handle(SendAbsenceEmailCommand request, CancellationToken cancellationToken)
        {
            Employee? employee = await _context.Employees
              .Where(e => e.Id == request.EmployeeId).FirstOrDefaultAsync(cancellationToken);
            string[] receiverEmail = { request.Email } ;
            string subject = request.Subject;
            string message = request.Message;
            bool status = SendMail(_senderEmail, _senderName, receiverEmail, message, subject);
            return true;
        }
        private bool SendMail(string senderEmail, string senderName, string[] receiverEmail, string message, string subject)
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
                List<SendSmtpEmailTo> toList = receiverEmail.Select(email => new SendSmtpEmailTo(email)).ToList();
                string htmlMessage = $@"
                <html>
                    <body>
                        <p style='color: black;'>{message}</p>
                        <br />
                        <p style='color: black;'>Regards,<br />{senderName}</p>
                    </body>
                </html>";


                SendSmtpEmail sendSmtpEmail = new SendSmtpEmail
                {
                    Sender = sender,
                    To = toList,
                    Subject = subject,
                    HtmlContent=htmlMessage

                };

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