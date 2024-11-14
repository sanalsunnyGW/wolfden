//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using WolfDen.Application.Requests.Commands.Attendence.Email;
//using WolfDen.Application.Requests.DTOs.Attendence;
//using WolfDen.Infrastructure.Data;


//namespace WolfDen.Application.Requests.Queries.Attendence.AttendenceLog
//{

//    public class AttendenceLogQueryHandler : IRequestHandler<AttendenceLogQuery, List<AttendenceLogDTO>>
//    {
//        private readonly WolfDenContext _context;
//        private readonly string SenderEmail;
//        private readonly string SenderName;
//        public AttendenceLogQueryHandler(WolfDenContext context,IConfiguration configuration)
//        {
//            _context = context;
//            SenderEmail= configuration["BrevoApi:SenderEmail"];
//            SenderName = configuration["BrevoApi:SenderName"];
//        }
//        public async Task<List<AttendenceLogDTO>> Handle(AttendenceLogQuery request, CancellationToken cancellationToken)
//        {
//            List<string> cCManagerEmail = new List<string>();

//            List<string> cCManagerName = new List<string>();
//            var attendenceRecords = await _context.AttendenceLog.Where(x => x.EmployeeId == request.EmployeeId && x.Date == request.Date).Include(x => x.Device)
//                .Select(x => new AttendenceLogDTO
//                {
//                    Time = x.Time,
//                    DeviceName = x.Device.Name,
//                    Direction = x.Direction
//                }).ToListAsync();

//            var employeeEmail = await _context.Employees.Where(x=>x.EmployeeId==request.EmployeeId).Select(x => x.Email).FirstOrDefaultAsync();
//            var employeeName= await _context.Employees.Where(x => x.EmployeeId == request.EmployeeId).Select(x => x.FirstName+" "+x.LastName).FirstOrDefaultAsync();

         
//            var managerId = await _context.Employees.Where(x => x.EmployeeId == request.EmployeeId).Select(x => x.ManagerId).FirstOrDefaultAsync();
//            if(managerId!=null)
//            {
//                var managers=await FindManager(managerId,cancellationToken);
//                cCManagerEmail.AddRange(managers);
//                cCManagerName.AddRange(managers);
//            }



//            string Subject = "Attendence Low Warning";
//            string Message = "Your working hour is less than hr";

            
//            SendEmailMethod.SendMail(SenderEmail, SenderName, employeeEmail, employeeName, Subject, Message, cCManagerEmail.ToArray(), cCManagerName.ToArray());
//            return attendenceRecords;


//        }
//        private async Task FindManager(int? managerId,CancellationToken cancellationToken)
//        {
//            List<string> cCManagerEmail=new List<string>();

//            List<string> cCManagerName = new List<string>();
//            var manager =await _context.Employees.Where(x => x.EmployeeId == managerId).Select(x => new 
//            {
               
//                x.Email,
//                x.ManagerId,
//                x.FirstName,
//                x.LastName
               
//            }).FirstOrDefaultAsync();
//            if (manager != null)
//            {
//                cCManagerEmail.Add(manager.Email);
//                cCManagerName.Add(manager.FirstName+" "+ manager.LastName);

//            }
//            if (manager.ManagerId != null)
//            {
//                await FindManager(manager.ManagerId,cancellationToken);

//            }
          

//            return (cCManagerEmail, cCManagerName); 
//        }

//    }
//}
