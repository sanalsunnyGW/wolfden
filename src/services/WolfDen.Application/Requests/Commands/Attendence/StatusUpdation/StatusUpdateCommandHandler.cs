using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WolfDen.Application.Requests.Commands.Attendence.Email;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Attendence.StatusUpdation
{
    public class StatusUpdateCommandHandler:IRequestHandler<StatusUpdateCommand,int>
    {
        private readonly WolfDenContext _context;
        private readonly string SenderEmail;
        private readonly string SenderName;
        public StatusUpdateCommandHandler(WolfDenContext context, IConfiguration configuration)
        {
            _context = context;
            SenderEmail = configuration["BrevoApi:SenderEmail"];
            SenderName = configuration["BrevoApi:SenderName"];

        }
        public async Task<int> Handle(StatusUpdateCommand request, CancellationToken cancellationToken)
        {
            int statusId = 0;
            var day = await _context.DailyAttendence.Where(x => x.EmployeeId == request.EmployeeId && x.Date == request.Date).FirstOrDefaultAsync(cancellationToken);
            if (day == null)
            {
                var holiday = await _context.Holiday.Where(x => x.Date == request.Date).FirstOrDefaultAsync();
              
                    if (holiday.Type == "Normal")
                    {
                        statusId = 5;
                    }
                    else
                    {
                        //var leave = await _context.LeaveRequest.Where(x => x.EmpId == request.EmployeeId && x.FromDate == request.Date && x.status == 2).FirstOrDefaultAsync(cancellationToken);
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
                List<string> cCManagerName = new List<string>();
                string[] recieverEmail = new string[1];
                string[] recieverName = new string[1];

                var recEmail = await _context.Employees.Where(x => x.Id == request.EmployeeId)
               .Select(x => x.Email).FirstOrDefaultAsync();

              
                var recName = await _context.Employees.Where(x => x.Id == request.EmployeeId)
                    .Select(x => x.FirstName + " " + x.LastName).FirstOrDefaultAsync();

                if (recEmail != null)
                {
                    recieverEmail[0] = recEmail;
                    recieverName[0] = recName;
                }

                var managerId = await _context.Employees.Where(x => x.Id== request.EmployeeId)
                    .Select(x => x.ManagerId).FirstOrDefaultAsync();

                if (managerId != null)
                {
                    var managers = await FindManager(managerId, cancellationToken);
                    cCManagerEmail.AddRange(managers.cCManagerEmail);
                    cCManagerName.AddRange(managers.cCManagerName);
                }

                string subject = "Attendence Low Warning";
                string message = $"{recieverName}'s working hour is less than required";
                SendEmailCommand.SendMail(SenderEmail, SenderName, recieverEmail, recieverName, subject, message, cCManagerEmail.ToArray(), cCManagerName.ToArray());

            }

            Status status = new Status(request.EmployeeId, request.Date, statusId);

            await _context.AddAsync(status);
            await _context.SaveChangesAsync();
            return status.Id;

        }
        private async Task<(List<string> cCManagerEmail, List<string> cCManagerName)> FindManager(int? managerId, CancellationToken cancellationToken)
        {
            List<string> cCManagerEmail = new List<string>();
            List<string> cCManagerName = new List<string>();

            var manager = await _context.Employees.Where(x => x.Id == managerId)
                .Select(x => new
                {
                    x.Email,
                    x.ManagerId,
                    x.FirstName,
                    x.LastName

                }).FirstOrDefaultAsync();
            if (manager != null)
            {
                cCManagerEmail.Add(manager.Email);
                cCManagerName.Add(manager.FirstName + " " + manager.LastName);
                if (manager.ManagerId != null)
                {
                    await FindManager(manager.ManagerId, cancellationToken);
                }
            }
            return (cCManagerEmail, cCManagerName);
        }

    }

}

