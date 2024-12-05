using System;
using Azure.Core.Serialization;
using LanguageExt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using WolfDen.Application.DTOs.Attendence;
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
            List<string> managerEmails = await _emailFinder.FindManagerEmailsAsync(employee.ManagerId);
            string subject = request.Subject;
            int? hours = request.Duration / 60;
            int? minutes = request.Duration % 60;
            string duration = $"{hours}h {minutes}m";
            var dynamicData = new Dictionary<string, object>
        {
            { "name", request.Name },
            {"date",request.Date },  
            { "arrivalTime",request.ArrivalTime.ToString("HH:mm:ss") },
            { "departureTime", request.DepartureTime.ToString("HH:mm:ss") },
            { "status", request.Status },
            { "duration", duration},
            { "message",request.Message},
            { "missedPunch",request.MissedPunch }

        };
            int templateId = 10;
            bool status = SendMail(_senderEmail, _senderName, receiverEmails,templateId, dynamicData, managerEmails.ToArray(),subject);
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
        private bool SendMail(string senderEmail, string senderName, string[] receiverEmails, int templateId, Dictionary<string, object> templateParams, string[]? ccEmails, string subject)
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
                var sendSmtpEmail = new SendSmtpEmail
                {
                    Sender = sender,
                    To = toList,
                    Cc = ccList,
                    TemplateId = templateId,
                    Params = templateParams,
                    Subject=subject
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