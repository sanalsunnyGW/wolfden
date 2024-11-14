﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.Requests.DTOs.LeaveManagement;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.LeaveManagementQuery.LeaveBalanceQuery
{
    public class GetLeaveBalanceQueryHandler : IRequestHandler<GetLeaveBalanceQuery, List<LeaveBalanceDto>>
    {
        private readonly WolfDenContext _context;

        public GetLeaveBalanceQueryHandler(WolfDenContext context)
        {
            _context=context;
        }
        public async Task<List<LeaveBalanceDto>> Handle(GetLeaveBalanceQuery request, CancellationToken cancellationToken)
        {
            List<LeaveBalance> LeaveBalanceList = await _context.LeaveBalances.Where(x => x.EmployeeId.Equals(request.RequestId))
            .Include(x=>x.LeaveType).ToListAsync();
            List<LeaveBalanceDto> leaveBalanceDtosList = new List<LeaveBalanceDto>();
            foreach(LeaveBalance leave in LeaveBalanceList)
            {
               LeaveBalanceDto leaveBalanceDto = new LeaveBalanceDto();
                leaveBalanceDto.Name = "casual";/*leave.LeaveType.TypeName;*/
                leaveBalanceDto.Balance = 0; /*leave.Balance;*/
                leaveBalanceDtosList.Add(leaveBalanceDto);  
            }
            return leaveBalanceDtosList;
         
        }
    }
}