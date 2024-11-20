using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveTypes
{
    public class GetAllLeaveTypeIdAndNameQuery : IRequest<List<LeaveTypeDto>>
    {
        
    }
}
