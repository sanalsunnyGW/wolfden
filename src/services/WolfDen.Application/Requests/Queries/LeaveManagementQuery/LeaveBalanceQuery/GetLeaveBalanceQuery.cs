using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.Requests.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Queries.LeaveManagementQuery.LeaveBalanceQuery
{
    public class GetLeaveBalanceQuery:IRequest<List<LeaveBalanceDto>>
    {
        public int RequestId {  get; set; } 
    }
}
