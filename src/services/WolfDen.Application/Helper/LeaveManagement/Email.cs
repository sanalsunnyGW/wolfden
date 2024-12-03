
using Microsoft.Extensions.Configuration;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace WolfDen.Application.Helper.LeaveManagement
{
    public class Email(IConfiguration configuration)
    {
        private readonly string _apiKey = configuration["BrevoApi:ApiKey"];


        public bool SendMail(string senderEmail, string senderName, string[] receiverEmails  , string message , string subject , string[] ccEmails = null)
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
                List<SendSmtpEmailCc>? ccList = ccEmails?.Select(email => new SendSmtpEmailCc(email)).ToList() ?? null;
                SendSmtpEmail sendSmtpEmail = new SendSmtpEmail
                {
                    Sender = sender,
                    To = toList,
                    Cc = ccList,
                    HtmlContent = message, 
                    Subject = subject
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
