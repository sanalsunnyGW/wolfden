using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WolfDen.Application.Requests.Commands.Attendence.Email;
using WolfDen.Application.Requests.DTOs;
using WolfDen.Infrastructure.Data;


namespace WolfDen.Application.Requests.Queries.Attendence.AttendenceLog
{

    public class AttendenceLogQueryHandler : IRequestHandler<AttendenceLogQuery, List<AttendenceLogDTO>>
    {
        private readonly WolfDenContext _context;
        private readonly string SenderEmail;
        private readonly string SenderName;
        public AttendenceLogQueryHandler(WolfDenContext context,IConfiguration configuration)
        {
            _context = context;
            SenderEmail= configuration["BrevoApi:SenderEmail"];
            SenderName = configuration["BrevoApi:SenderName"];
        }
        public async Task<List<AttendenceLogDTO>> Handle(AttendenceLogQuery request, CancellationToken cancellationToken)
        {

            var attendenceRecords = await _context.AttendenceLog.Where(x => x.EmployeeId == request.EmployeeId && x.Date == request.Date).Include(x => x.Device)
                .Select(x => new AttendenceLogDTO
                {
                    Time = x.Time,
                    DeviceName = x.Device.Name,
                    Direction = x.Direction
                }).ToListAsync();

            string RecieverEmail = "athullya.r@geekywolf.com";
            string RecieverName = "Athullya";
            string Subject = "Attendence Low Warning";
            string Message = "Your working hour is less than hr";
            SendEmailMethod.SendMail(SenderEmail,SenderName,RecieverEmail, RecieverName, Subject, Message);
            return attendenceRecords;


        }


    }
}
