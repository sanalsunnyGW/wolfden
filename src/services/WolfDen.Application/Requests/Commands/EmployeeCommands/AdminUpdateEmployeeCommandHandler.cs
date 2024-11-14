using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.EmployeeCommands
{
    public class AdminUpdateEmployeeCommandHandler : IRequestHandler<AdminUpdateEmployeeCommand, bool>
    {
        private readonly WolfDenContext _context;

        public AdminUpdateEmployeeCommandHandler(WolfDenContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AdminUpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = _context.Employees.Find(request.Id);
            if (employee == null)
            {
                return false;
            }
            employee.AdminUpdateEmployee(request.DesignationId,request.DepartmentId,request.ManagerId,request.IsActive);
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
