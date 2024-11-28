using Microsoft.EntityFrameworkCore;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Helpers
{
    public class ManagerEmailFinder
    {
        private readonly WolfDenContext _context;

        public ManagerEmailFinder(WolfDenContext context)
        {
            _context = context;
        }
        public async Task<List<string>> FindManagerEmailsAsync(int? managerId, CancellationToken cancellationToken)
        {
            List<string> managerEmails = new List<string>();
            if (managerId is null) 
                return managerEmails;
            Employee? manager = await _context.Employees
                .Where(m => m.Id == managerId)
                .FirstOrDefaultAsync(cancellationToken);
            if (manager is not null)
            {
                managerEmails.Add(manager.Email);
                List<string> higherManagerEmails = await FindManagerEmailsAsync(manager.ManagerId, cancellationToken);
                managerEmails.AddRange(higherManagerEmails);
            }
            return managerEmails;
        }
    }
}