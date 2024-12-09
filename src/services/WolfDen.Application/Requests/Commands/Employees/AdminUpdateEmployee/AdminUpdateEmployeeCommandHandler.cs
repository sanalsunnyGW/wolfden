using MediatR;
using System.ComponentModel.DataAnnotations;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Employees.AdminUpdateEmployee
{
    public class AdminUpdateEmployeeCommandHandler(WolfDenContext context, AdminUpdateEmployeeValidator validator) : IRequestHandler<AdminUpdateEmployeeCommand, bool>
    {
        private readonly WolfDenContext _context = context;
        private readonly AdminUpdateEmployeeValidator _validator = validator;

        public async Task<bool> Handle(AdminUpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }

            Employee employee = await _context.Employees.FindAsync(request.Id, cancellationToken);
            if (employee == null)
            {
                return false;
            }
            employee.AdminUpdateEmployee(request.DesignationId, request.DepartmentId, request.ManagerId, request.IsActive, request.JoiningDate, request.EmploymentType);
            _context.Employees.Update(employee);
            if (request.IsActive is false) 
            {
                List<Employee> myTeam=_context.Employees.Where(x=>x.ManagerId==request.Id).ToList();
                if (myTeam.Any())
                {
                    foreach (var team in myTeam) 
                    { 
                        team.UpdateManager(employee.ManagerId);
                    }

                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
