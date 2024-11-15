using EmployeeManagementDomain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Domain.Entity
{
    public class Employee
    {
        public int Id { get; private set; }
        public int EmployeeId { get; private set; }

        public string RFId { get; private set; }
        public string FirstName { get; private set; }
        public string? LastName { get; private set; }

        public int? Age { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }

        public DateOnly? JoiningDate { get; private set; }
        public int DesignationId { get; private set; }
        public Designation Designation { get; private set; }
        public int DepartmentId {  get; private set; }
        public Department Department { get; private set; }
        public int? ManagerId { get; private set; }
        public virtual Employee Manager {  get; private set; }


        public Employee()
        {

        }

        public Employee(int employeeId, string rfId, string firstName, string lastName, int age, string email, string phoneNumber, DateOnly joiningDate, int designationId,int departmentId,int managerId)
        {
            EmployeeId = employeeId;
            RFId = rfId;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Email = email;
            PhoneNumber = phoneNumber;
            JoiningDate = joiningDate;
            DesignationId = designationId;
            DepartmentId = departmentId;
            ManagerId = managerId;

        }

    } 
}

