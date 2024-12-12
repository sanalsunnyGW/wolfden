using Moq;
using Moq.EntityFrameworkCore;
using WolfDen.Application.Requests.Queries.Employees.EmployeeDirectory;
using WolfDen.Infrastructure.Data;

namespace WolfDenTests.QueryHandlerTests.EmployeeTests
{
    public class GetAllEmployeeQueryHandlerTests
    {
        /*private readonly Mock<WolfDenContext> _mockContext;
        private readonly GetAllEmployeeQueryHandler _handler;

        public GetAllEmployeeQueryHandlerTests()
        {
            _mockContext = new Mock<WolfDenContext>();
            _handler = new GetAllEmployeeQueryHandler(_mockContext.Object);
        }


         [Fact]
        public async Task Handle_ShouldReturnPaginatedEmployeeList_WhenDataExists()
        {
            // Arrange
            var employees = new List<WolfDen.Domain.Entity.Employee>
            {
                new WolfDen.Domain.Entity.Employee(1, "RF123", "user1"),
                new WolfDen.Domain.Entity.Employee(2, "RF124", "user2")
            };

            var department = new WolfDen.Domain.Entity.Department("Development");
            var designation = new WolfDen.Domain.Entity.Designation("Senior Dev");

            _mockContext.Setup(x => x.Employees);
            _mockContext.Setup(x => x.Departments);
            _mockContext.Setup(x => x.Designations)
                .ReturnsDbSet(new List<WolfDen.Domain.Entity.Designation> { designation });



            // Set up the mock context
            _mockContext.Setup(x => x.Employees).Returns(mockEmployeeDbSet.Object);
            _mockContext.Setup(x => x.Departments).Returns(mockDepartmentDbSet.Object);
            _mockContext.Setup(x => x.Designations).Returns(mockDesignationDbSet.Object);


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
