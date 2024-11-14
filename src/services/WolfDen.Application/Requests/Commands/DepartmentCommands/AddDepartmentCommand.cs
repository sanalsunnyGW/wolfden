using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Application.Requests.Commands.DeparmentCommands
{
    public class AddDepartmentCommand : IRequest<int>
    {
        public string DepartmentName{ get; set; }

    }
}
