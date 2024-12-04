using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Services
{
    public class ManagerIdFinder
    {
        private readonly WolfDenContext _context;

        public ManagerIdFinder(WolfDenContext context)
        {
            _context = context;
        }

        public async Task<List<int>> FindManagerIdsAsync(int? managerId)
        {
            List<int> managerIds = new List<int>();
            if (managerId is null)
                return managerIds;

            Employee? manager = await _context.Employees.FindAsync(managerId);
            if (manager is not null)
            {
                managerIds.Add(manager.Id);
                List<int> higherManagerIds = await FindManagerIdsAsync(manager.ManagerId);
                managerIds.AddRange(higherManagerIds);
            }
            return managerIds;
        }
    }
}
