using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetLeaveRequestHistory
{
    public class GetLeaveRequestHistoryQueryHandler : IRequestHandler<GetLeaveRequestHistoryQuery, List<LeaveRequestDto>>
    {
        private readonly WolfDenContext _context;

        public GetLeaveRequestHistoryQueryHandler(WolfDenContext context)
        {
            _context = context;
        }

        public async Task<List<LeaveRequestDto>> Handle(GetLeaveRequestHistoryQuery request, CancellationToken cancellationToken)
        {
            List<LeaveRequest> leaveRequests = await _context.LeaveRequests
                  .Where(x => x.EmployeeId.Equals(request.RequestId))
                 .Include(x => x.LeaveType).ToListAsync(cancellationToken);

            List<LeaveRequestDto> leaveRequestList=new List<LeaveRequestDto>(); 

            foreach (LeaveRequest leaveRequest in leaveRequests)
            {
                LeaveRequestDto leaveRequestDto = new LeaveRequestDto();
                leaveRequestDto.FromDate=leaveRequest.FromDate;
                leaveRequestDto.ToDate=leaveRequest.ToDate;  
                leaveRequestDto.ApplyDate=leaveRequest.ApplyDate;
                leaveRequestDto.TypeName = leaveRequest.LeaveType.TypeName;
                leaveRequestDto.HalfDay = leaveRequest.HalfDay;
                leaveRequestDto.Description = leaveRequest.Description;
                 var approverName=await _context.Employees.Where(x=>x.Id.Equals(leaveRequest.ProcessedBy)).Select(x=>x.FirstName).FirstOrDefaultAsync(cancellationToken);
                leaveRequestDto.ProcessedBy = approverName;
                leaveRequestDto.LeaveRequestStatus = leaveRequest.LeaveRequestStatusId;
                leaveRequestList.Add(leaveRequestDto);  
            }
            return leaveRequestList;
        }
    }
}
