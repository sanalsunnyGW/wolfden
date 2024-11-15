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

namespace WolfDen.Application.Requests.Queries.LeaveManagementQuery.LeaveBalanceQuery
{
    public class GetLeaveBalanceQueryHandler : IRequestHandler<GetLeaveBalanceQuery, List<object>>
    {
        private readonly WolfDenContext _context;

        public GetLeaveBalanceQueryHandler(WolfDenContext context)
        {
            _context=context;
        }
        public async Task<List<object>> Handle(GetLeaveBalanceQuery request, CancellationToken cancellationToken)
        {
            List<LeaveBalance> LeaveBalanceList = await _context.LeaveBalances.Where(x => x.EmployeeId.Equals(request.RequestId))
            .Include(x=>x.LeaveType).ToListAsync();
            List<LeaveBalanceDto> leaveBalanceDtosList = new List<LeaveBalanceDto>();
            foreach(LeaveBalance leave in LeaveBalanceList)
            {
               LeaveBalanceDto leaveBalanceDto = new LeaveBalanceDto();
                leaveBalanceDto.Name = leave.LeaveType.TypeName;
                leaveBalanceDto.Balance = leave.Balance;
                leaveBalanceDtosList.Add(leaveBalanceDto);  
            }
            var listofobjects=new List<object> { leaveBalanceDtosList };
            //return leaveBalanceDtosList;
            return listofobjects;
         
        }
    }
}
