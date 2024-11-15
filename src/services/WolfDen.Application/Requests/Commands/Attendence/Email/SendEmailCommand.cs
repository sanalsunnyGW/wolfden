using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;

namespace WolfDen.Application.Requests.Commands.Attendence.Email
{
    public class SendEmailCommand
    {
        public static void SendMail(string senderEmail, string senderName, string[] recieverEmail, string[] recieverName,string message, string subject, string[] ccEmails=null, string[] ccNames=null)
        {
            var apiInstance = new TransactionalEmailsApi();
            SendSmtpEmailSender sender = new SendSmtpEmailSender(senderName, senderEmail);

            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            if (recieverEmail != null)
                for (int i = 0; i < recieverEmail.Length; i++)
                {
                    To.Add(new SendSmtpEmailTo(recieverEmail[i], recieverName[i]));
                }
         

            List<SendSmtpEmailCc> Cc = new List<SendSmtpEmailCc>();
            if(ccEmails != null)
            for (int i = 0; i < ccEmails.Length; i++)
            {
                Cc.Add(new SendSmtpEmailCc(ccEmails[i], ccNames[i]));
            }
            string HtmlContent = null;
            string TextContent = message;
            try
            {
                var sendSmtpEmail = new SendSmtpEmail(sender, To, null, Cc, HtmlContent, TextContent, subject);
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
