using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Domain.Entity;

namespace EmployeeManagementDomain.Entity
{
    public class SuperAdmin
    {
        public int Id { get;private set; }
        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; }
        public SuperAdmin() { }
        public SuperAdmin(int employeeId)
        { 
            EmployeeId=employeeId;

        }
    }
}
