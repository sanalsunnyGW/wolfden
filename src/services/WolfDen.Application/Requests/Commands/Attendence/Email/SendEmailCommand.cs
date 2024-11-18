using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;

namespace WolfDen.Application.Requests.Commands.Attendence.Email
{
    public class SendEmailCommand
    {
        public static void SendMail(string senderEmail,string senderName,
            string[] recieverEmail,
            string message,
            string subject,
            string[] ccEmails=null)
           
        {
            TransactionalEmailsApi apiInstance = new TransactionalEmailsApi();

            SendSmtpEmailSender sender = new SendSmtpEmailSender
            {
                Email = senderEmail,
                Name = senderName
            };

            List<SendSmtpEmailTo> to = new List<SendSmtpEmailTo>();
            if (recieverEmail != null)
                for (int i = 0; i < recieverEmail.Length; i++)
                {
                    to.Add(new SendSmtpEmailTo(recieverEmail[i]));
                }
         

            List<SendSmtpEmailCc> cc = new List<SendSmtpEmailCc>();
            if(ccEmails != null)
            for (int i = 0; i < ccEmails.Length; i++)
            {
                cc.Add(new SendSmtpEmailCc(ccEmails[i]));
            }
            string HtmlContent = null;
            string TextContent = message;
            try
            {
                SendSmtpEmail sendSmtpEmail = new SendSmtpEmail(sender, to, null, cc, HtmlContent, TextContent, subject);
                CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
                Console.WriteLine("Response" + result.ToJson());
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e.Message);
            }
        }
    }
}
