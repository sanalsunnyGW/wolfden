using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.EmployeeCommands
{
    public class EmployeeUpdateEmployeeCommandHandler:IRequestHandler<EmployeeUpdateEmployeeCommand,bool>
    {
        private readonly WolfDenContext _context;

        public EmployeeUpdateEmployeeCommandHandler(WolfDenContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(EmployeeUpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee=_context.Employees.Find(request.Id);
            if (employee == null) 
            { 
                return false; 
            }
            employee.EmployeeUpdateEmployee(request.FirstName, request.LastName, request.DateofBirth, request.Email, request.PhoneNumber, request.Gender, request.JoiningDate);
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return true;
            
            
            

        }
    }
}
