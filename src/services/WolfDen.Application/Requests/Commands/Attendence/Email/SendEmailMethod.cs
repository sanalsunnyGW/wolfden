using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;

namespace WolfDen.Application.Requests.Commands.Attendence.Email
{
    public class SendEmailMethod
    {
        public static void SendMail(string senderEmail, string senderName, string recieverEmail, string recieverName,string message, string subject, string[] ccEmails=null, string[] ccNames=null)
        {
            var apiInstance = new TransactionalEmailsApi();
            SendSmtpEmailSender Sender = new SendSmtpEmailSender(senderName, senderEmail);
            SendSmtpEmailTo Reciever1 = new SendSmtpEmailTo(recieverEmail, recieverName);
            List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
            To.Add(Reciever1);

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
                var sendSmtpEmail = new SendSmtpEmail(Sender, To, null, Cc, HtmlContent, TextContent, subject);
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
