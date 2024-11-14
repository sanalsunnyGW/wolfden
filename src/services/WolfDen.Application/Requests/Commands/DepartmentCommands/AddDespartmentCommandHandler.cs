using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.Requests.Commands.DeparmentCommands;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.DepartmentCommands

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
            Department department= new Department(request.DepartmentName);
            _context.Departments.Add(department);
            return await _context.SaveChangesAsync();
        }
    }
}
