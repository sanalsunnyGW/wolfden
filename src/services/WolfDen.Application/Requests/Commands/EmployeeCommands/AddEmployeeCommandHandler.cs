using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.EmployeeCommands
{
    public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand,int>
    {
        private readonly WolfDenContext _context;

        public AddEmployeeCommandHandler(WolfDenContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            Employee Employee = new Employee(request.EmployeeCode, request.RFId,request.FirstName);
            _context.Employees.Add(Employee);
            return await _context.SaveChangesAsync();
        }
    }
}
