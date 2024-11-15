using FluentValidation;
using MediatR;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Employees.AddEmployee
{
    public class AddEmployeeCommandHandler(WolfDenContext context, CreateEmployeeValidator validator) : IRequestHandler<AddEmployeecommand, int>
    {
        private readonly WolfDenContext _context = context;
        private readonly CreateEmployeeValidator _validator = validator;

        public async Task<int> Handle(AddEmployeecommand request, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }

            Employee Employee = new Employee(request.EmployeeCode, request.RFId, request.FirstName);
            _context.Employees.Add(Employee);
            await _context.SaveChangesAsync(cancellationToken);

            return Employee.Id;
        }
    }
}
