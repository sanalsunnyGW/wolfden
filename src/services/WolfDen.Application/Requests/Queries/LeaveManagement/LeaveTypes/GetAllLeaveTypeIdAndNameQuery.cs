using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveTypes
{
    public class GetAllLeaveTypeIdAndNameQuery : IRequest<List<LeaveTypeDto>>
        {

        }
}
