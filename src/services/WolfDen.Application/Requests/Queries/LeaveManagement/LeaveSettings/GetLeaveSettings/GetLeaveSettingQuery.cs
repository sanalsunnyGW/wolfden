using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveSettings.GetLeaveSettings
{
    public class GetLeaveSettingQuery:IRequest<LeaveSettingDto>
    {
    }
}
