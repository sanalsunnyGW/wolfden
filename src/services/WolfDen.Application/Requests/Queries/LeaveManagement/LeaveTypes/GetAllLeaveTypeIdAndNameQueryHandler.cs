using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveTypes
{
    public class GetAllLeaveTypeIdAndNameQueryHandler : IRequestHandler<GetAllLeaveTypeIdAndNameQuery, List<LeaveTypeDto>>
    {
        public Task<List<LeaveTypeDto>> Handle(GetAllLeaveTypeIdAndNameQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
