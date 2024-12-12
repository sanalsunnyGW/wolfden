using MediatR;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Employees.TeamManagerUpdate
{
    public class TeamManagerUpdateCommandHandler(WolfDenContext context) : IRequestHandler<TeamManagerUpdateCommand, bool>
    {
        private readonly WolfDenContext _context = context;
        public async Task<bool> Handle(TeamManagerUpdateCommand request, CancellationToken cancellationToken)
        {
            Employee employee = await _context.Employees.FindAsync(request.Id, cancellationToken);
            if (employee != null)
            {
                employee.UpdateManager(request.ManagerId);
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync(cancellationToken);
                return true;
            }
            return false;
        }
    }
}
