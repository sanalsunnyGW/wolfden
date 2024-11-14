using MediatR;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Departments

{
    public class AddDespartmentCommandHandler : IRequestHandler<AddDepartmentCommand, int>
    {
        private readonly WolfDenContext _context;

        public AddDespartmentCommandHandler(WolfDenContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
            Department department = new Department(request.DepartmentName);
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department.Id;
        }
    }
}
