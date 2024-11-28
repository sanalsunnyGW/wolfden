using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Application.Requests.Commands.Employees.SuperAdminUpdateEmployee
{
    public class SuperAdminUpdateEmployeeCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string? Role { get; set; }
    }
}
