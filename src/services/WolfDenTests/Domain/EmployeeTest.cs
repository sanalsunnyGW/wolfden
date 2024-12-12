using Microsoft.Owin.BuilderProperties;
using System.Diagnostics.Metrics;
using WolfDen.Domain.Entity;
using static WolfDen.Domain.Enums.EmployeeEnum;

namespace WolfDenTests.EntityTests
{
    public class EmployeeTest
    {
        

        [Fact]
        public void Constructor_ShouldInitializeEmployeeCorrectly()
        {
            // arrange
            int employeeCode = 123;
            string rfId = "RF123";
            string userId = "user123";

            // act
            var employee = new Employee(employeeCode, rfId, userId);

            // assert
            Assert.Equal(employeeCode, employee.EmployeeCode);
            Assert.Equal(rfId, employee.RFId);
            Assert.Equal(userId, employee.UserId);
        }

        [Fact]
        public void EmployeeUpdateEmployee_ShouldUpdateEmployeeProperties()
        {
            int employeeCode = 123;
            string rfId = "RF123";
            string userId = "user123";
            var employee = new Employee(employeeCode, rfId, userId);

            //arrange
            string FirstName ="Nohan";
            string LastName = "Antony";
            DateOnly DateOfBirth = new DateOnly(2015, 12, 31);
            string Email = "nohan@gmail.com";
            string PhoneNumber = "8999999905";
            Gender Gender = Gender.Male;
            string Address = null;
            string Country = null;
            string State = null;
            string Photo = null;

            //act
           employee.EmployeeUpdateEmployee(FirstName, LastName, DateOfBirth, Email, PhoneNumber, Gender, Address, Country, State, Photo);

            //asert
            Assert.Equal(FirstName, employee.FirstName);
            Assert.Equal(LastName, employee.LastName);
            Assert.Equal(DateOfBirth, employee.DateofBirth);
            Assert.Equal(Email, employee.Email);
            Assert.Equal(PhoneNumber, employee.PhoneNumber);
            Assert.Equal(Gender, employee.Gender);
            Assert.Equal(Address, employee.Address);
            Assert.Equal(Country, employee.Country);
            Assert.Equal(State, employee.State);
            Assert.Equal(Photo, employee.Photo);
        }
        [Fact]
        public void AdminUpdateEmployee_ShouldUpdateEmployeeProperties()
        {
            int employeeCode = 123;
            string rfId = "RF123";
            string userId = "user123";
            var employee = new Employee(employeeCode, rfId, userId);

            //arrange
            int designationId = 1;
            int departmentId = 1;
            int managerId = 1;
            bool isActive = true;
            DateOnly joiningDate = new DateOnly(2020,05,24);
            EmploymentType employmentType=EmploymentType.Permanent;

            //act
            employee.AdminUpdateEmployee(designationId, departmentId, managerId, isActive, joiningDate, employmentType);

            //assert
            Assert.Equal(designationId, employee.DesignationId);
            Assert.Equal(departmentId, employee.DepartmentId);
            Assert.Equal(managerId, employee.ManagerId);
        }
    }
}
