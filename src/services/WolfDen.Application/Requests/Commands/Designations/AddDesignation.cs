using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Application.Requests.Commands.Designations
{
    public class AddDesignation : IRequest<int>
    {
        public string DesignationName { get; set; }
    }
}
