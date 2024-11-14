using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Application.Requests.Commands.EmployeeCommands
{
    public class AddEmployeeCommand:IRequest<int>
    {
        public int EmployeeCode { get; set; }
        public string RFId { get; set; }
        public string FirstName { get; set; }


    }
}
