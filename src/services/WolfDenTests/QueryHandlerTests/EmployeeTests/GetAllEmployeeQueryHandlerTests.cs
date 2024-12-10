using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.Requests.Queries.Employees.EmployeeDirectory;
using WolfDen.Infrastructure.Data;

namespace WolfDenTests.QueryHandlerTests.EmployeeTests
{
    public class GetAllEmployeeQueryHandlerTests
    {
        private readonly Mock<WolfDenContext> _mockContext;
        private readonly GetAllEmployeeQueryHandler _handler;

        public GetAllEmployeeQueryHandlerTests()
        {
            _mockContext = new Mock<WolfDenContext>();
            _handler = new GetAllEmployeeQueryHandler(_mockContext.Object);
        }


/*        [Fact]
        public async Task Handle_ShouldReturnPaginatedEmployeeList_WhenDataExists()
        {
            // Arrange
            var employees = new List<WolfDen.Domain.Entity.Employee>
            {
                new WolfDen.Domain.Entity.Employee(1, "RF123", "user1"),
                new WolfDen.Domain.Entity.Employee(2, "RF124", "user2")
            };

            var department = new WDepartment();
            var designation = new WolfDen.Domain.Entity.Designation { Id = 1, Name = "Manager" };

            _mockContext.Setup(x => x.Employees)
                .ReturnsDbSet(employees);
            _mockContext.Setup(x => x.Departments)
                .ReturnsDbSet(new List<WolfDen.Domain.Entity.Department> { department });
            _mockContext.Setup(x => x.Designations)
                .ReturnsDbSet(new List<WolfDen.Domain.Entity.Designation> { designation });

            var request = new GetAllEmployeeQuery
            {
                PageNumber = 0,
                PageSize = 10
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.EmployeeDirectoryDTOs.Count);
            Assert.Equal(1, result.TotalPages);
            Assert.Equal(2, result.TotalRecords);
        }*/
    }
}
