using MediatR;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequestDays
{
    public class AddLeaveRequestDaysCommandHandler(WolfDenContext context): IRequestHandler<AddLeaveRequestDayCommand,bool>
    {
        private readonly WolfDenContext _context = context;

        public async Task<bool> Handle(AddLeaveRequestDayCommand request, CancellationToken cancellationToken)
        {
            foreach(DateOnly date in request.Date)
            {
                LeaveRequestDay leaveRequestDay = new LeaveRequestDay(request.LeaveRequestId,date);
                _context.Add(leaveRequestDay);

            }
            int result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}
