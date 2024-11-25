using static WolfDen.Domain.Enums.EmployeeEnum;


namespace WolfDen.Domain.Entity
{
    public class Employee
    {
        public int Id { get; private set; }
        public int EmployeeCode { get; private set; }
        public string? RFId { get; private set; }
        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? Email { get; private set; }
        public string? PhoneNumber { get; private set; }
        public DateOnly? DateofBirth { get; private set; }

        public DateOnly? JoiningDate { get; private set; }
        public Gender? Gender { get; private set; }
        public int? DesignationId { get; private set; }
        public Designation? Designation { get; private set; }
        public int? DepartmentId { get; private set; }
        public Department? Department { get; private set; }
        public int? ManagerId { get; private set; }
        public virtual Employee? Manager { get; private set; }
        public bool? IsActive { get; private set; }

        public string? Password {  get; private set; }


        private Employee()
        {

        }

        public Employee(int employeeCode, string rfId)
        {
            EmployeeCode = employeeCode;
            RFId = rfId;
        }

        public void EmployeeUpdateEmployee(string? firstName, string? lastName, DateOnly? dateofBirth, 
                                           string? email, string? phoneNumber, Gender? gender,string password)
        {
            FirstName = firstName;
            LastName = lastName;
            DateofBirth = dateofBirth;
            Email = email;
            PhoneNumber = phoneNumber;
            Gender = gender;
            Password = password;
        }
        public void AdminUpdateEmployee(int? designationId, int? departmentId, int? managerId, 
                                        bool? isActive, DateOnly? joiningDate)
        {
            DepartmentId = departmentId;
            DesignationId = designationId;
            ManagerId = managerId;
            IsActive = isActive;
            JoiningDate = joiningDate;
        }
    }
}

