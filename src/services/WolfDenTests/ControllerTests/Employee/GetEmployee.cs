using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Queries.Employees.ViewEmployee;
using WolfDen.API.Controllers.Employee;
using Microsoft.AspNetCore.Mvc;
using System;
using MediatR;
using static WolfDen.Domain.Enums.EmployeeEnum;

namespace WolfDenTests.ControllerTests.Employee
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly WolfDen.API.Controllers.Employee.Employee _controller;

        public EmployeeControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new WolfDen.API.Controllers.Employee.Employee(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetEmployee_ReturnsEmployeeDTO_WithAllProperties_WhenEmployeeIdIsValid()
        {
            // Arrange
            var employeeId = 1;
            var query = new GetEmployeeQuery { EmployeeId = employeeId };

            var expectedEmployee = new EmployeeDTO
            {
                Id = employeeId,
                RFId = "ABC1",
                EmployeeCode = 1001,
                FirstName = "Nohan",
                LastName = "Antony",
                Email = "nohan@gmail.com",
                PhoneNumber = "123-456-7890",
                DateofBirth = new DateOnly(2000, 5, 15),
                JoiningDate = new DateOnly(2020, 1, 1),
                Gender = Gender.Male,
                DesignationId = 101,
                DesignationName = "Software Engineer",
                DepartmentId = 10,
                DepartmentName = "Engineering",
                ManagerId = 2,
                ManagerName = "Sanal Sunny",
                IsActive = true,
                Address = "Kottayam",
                Country = "India",
                State = "Kerala",
                Photo = null,
                EmploymentType = EmploymentType.Permanent,
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetEmployeeQuery>(q => q.EmployeeId == employeeId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedEmployee);

            // Act
            var result = await _controller.GetEmployee(query, CancellationToken.None);

            // Assert
            Assert.IsType<EmployeeDTO>(result); 

            Assert.Equal(expectedEmployee.Id, result.Id);
            Assert.Equal(expectedEmployee.RFId, result.RFId);
            Assert.Equal(expectedEmployee.EmployeeCode, result.EmployeeCode);
            Assert.Equal(expectedEmployee.FirstName, result.FirstName);
            Assert.Equal(expectedEmployee.LastName, result.LastName);
            Assert.Equal(expectedEmployee.Email, result.Email);
            Assert.Equal(expectedEmployee.PhoneNumber, result.PhoneNumber);
            Assert.Equal(expectedEmployee.DateofBirth, result.DateofBirth);
            Assert.Equal(expectedEmployee.JoiningDate, result.JoiningDate);
            Assert.Equal(expectedEmployee.Gender, result.Gender);
            Assert.Equal(expectedEmployee.DesignationId, result.DesignationId);
            Assert.Equal(expectedEmployee.DesignationName, result.DesignationName);
            Assert.Equal(expectedEmployee.DepartmentId, result.DepartmentId);
            Assert.Equal(expectedEmployee.DepartmentName, result.DepartmentName);
            Assert.Equal(expectedEmployee.ManagerId, result.ManagerId);
            Assert.Equal(expectedEmployee.ManagerName, result.ManagerName);
            Assert.Equal(expectedEmployee.IsActive, result.IsActive);
            Assert.Equal(expectedEmployee.Address, result.Address);
            Assert.Equal(expectedEmployee.Country, result.Country);
            Assert.Equal(expectedEmployee.State, result.State);
            Assert.Equal(expectedEmployee.Photo, result.Photo);
            Assert.Equal(expectedEmployee.EmploymentType, result.EmploymentType);

            // Verify that Send method is called with the correct parameters
            _mediatorMock.Verify(m => m.Send(It.Is<GetEmployeeQuery>(q => q.EmployeeId == employeeId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEmployee_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var employeeId = 999; // Assume this ID doesn't exist in the system
            var query = new GetEmployeeQuery { EmployeeId = employeeId };

            // Simulate that the query returns null, indicating the employee does not exist
            _mediatorMock
                .Setup(m => m.Send(It.Is<GetEmployeeQuery>(q => q.EmployeeId == employeeId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((EmployeeDTO)null);

            // Act
            var result = await _controller.GetEmployee(query, CancellationToken.None);

            // Assert
            Assert.Null(result); // If no employee is found, the result should be null
            _mediatorMock.Verify(m => m.Send(It.Is<GetEmployeeQuery>(q => q.EmployeeId == employeeId), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
