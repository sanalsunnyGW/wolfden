using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WolfDen.Application.Requests.Commands.Attendence.Email;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Attendence.StatusUpdation
{
    public class StatusUpdateCommandHandler : IRequestHandler<StatusUpdateCommand, int>
    {
        private readonly WolfDenContext _context;
        private readonly string senderEmail;
        private readonly string senderName;
        public StatusUpdateCommandHandler(WolfDenContext context, IConfiguration configuration)
        {
            _context = context;
            senderEmail = configuration["BrevoApi:SenderEmail"];
            senderName = configuration["BrevoApi:SenderName"];
        }
        public async Task<int> Handle(StatusUpdateCommand request, CancellationToken cancellationToken)
        {
            int statusId = 0;
            DailyAttendence day = await _context.DailyAttendence.Where(x => x.EmployeeId == request.EmployeeId && x.Date == request.Date).FirstOrDefaultAsync(cancellationToken);
            if (day is null)
            {
                Holiday holiday = await _context.Holiday.Where(x => x.Date == request.Date).FirstOrDefaultAsync(cancellationToken);

                if (holiday.Type == "Normal")
                {
                    statusId = 5;
                }
                else
                {
                    //LeaveRequest leave = await _context.LeaveRequest.Where(x => x.EmpId == request.EmployeeId && x.FromDate == request.Date && x.status == 2).FirstOrDefaultAsync(cancellationToken);
                    //if (leave != null)
                    //{
                    //    if (leave.TypeName == "WFH")
                    //    {
                    //        statusId = 3;
                    //    }
                    //    else
                    //    {
                    statusId = 5;
                    //    }
                    //}
                }
            }
            else
            {
                var insideHrs = await _context.DailyAttendence.Where(x => x.EmployeeId == request.EmployeeId && x.Date == request.Date).Select(x => x.InsideDuration).FirstOrDefaultAsync(cancellationToken);
                if (insideHrs < 360)
                {
                    statusId = 2;
                }
                else
                {
                    statusId = 1;
                }
            }

            if (statusId == 2)
            {
                List<string> cCManagerEmail = new List<string>();

                string[] recieverEmail = new string[1];

                var recEmail = await _context.Employees.Where(x => x.Id == request.EmployeeId)
               .Select(x => x.Email).FirstOrDefaultAsync(cancellationToken);

                var recName = await _context.Employees.Where(x => x.Id == request.EmployeeId)
                    .Select(x => x.FirstName + " " + x.LastName).FirstOrDefaultAsync(cancellationToken);

                if (recEmail != null)
                {
                    recieverEmail[0] = recEmail;
                }

                var managerId = await _context.Employees.Where(x => x.Id == request.EmployeeId)
                    .Select(x => x.ManagerId).FirstOrDefaultAsync();

                if (managerId != null)
                {
                    cCManagerEmail = await FindManager(managerId, cancellationToken);
                }

                string subject = "Attendence Low Warning";
                string message = $"{recName}'s working hour is less than required";
                SendEmailCommand.SendMail(senderEmail, senderName, recieverEmail, message, subject, cCManagerEmail.ToArray());

            }

            Status status = new Status(request.EmployeeId, request.Date, statusId);

            await _context.AddAsync(status);
            await _context.SaveChangesAsync(cancellationToken);
            return status.Id;

        }
        private async Task<List<string>> FindManager(int? managerId, CancellationToken cancellationToken)
        {
            List<string> cCManagerEmail = new List<string>();

            var manager = await _context.Employees.Where(x => x.Id == managerId)
                .Select(x => new
                {
                    x.Email,
                    x.ManagerId
                }).FirstOrDefaultAsync();

            if (manager != null)
            {
               
                if (manager.ManagerId != null)
                {
                    var managerEmails=await FindManager(manager.ManagerId, cancellationToken);
                    cCManagerEmail.AddRange(managerEmails);
                }
            }
            return cCManagerEmail;
        }

    }

}

