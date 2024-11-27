using MediatR;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequestDays
{
    public class AddLeaveRequestDayCommand : IRequest<bool>
    {
       public int LeaveRequestId { get; set; }    
       public List<DateOnly> Date {  get; set; }  
    }
}
