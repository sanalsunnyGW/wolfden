using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveBalances.GetLeaveBalance
{
    public class GetLeaveBalanceQuery : IRequest<List<LeaveBalanceDto>>
    {
        public int RequestId { get; set; }
    }
}
