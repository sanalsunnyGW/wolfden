using MediatR;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Departments

{
    public class AddDespartmentCommandHandler(WolfDenContext context) : IRequestHandler<AddDepartmentCommand, int>
    {
        private readonly WolfDenContext _context=context;

        public async Task<int> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
            Department department = new Department(request.DepartmentName);
            _context.Departments.Add(department);
            await _context.SaveChangesAsync(cancellationToken);
            return department.Id;
        }
    }
}
