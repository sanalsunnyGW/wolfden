using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Application.Requests.Commands.Employees
{
    public class AdminUpdateEmployee : IRequest<bool>
    {
        public int Id { get; set; }
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ManagerId { get; set; }
        public bool? IsActive { get; set; }
    }
}
