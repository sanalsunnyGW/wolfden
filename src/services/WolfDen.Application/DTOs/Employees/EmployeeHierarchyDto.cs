using static WolfDen.Domain.Enums.EmployeeEnum;

namespace WolfDen.Application.DTOs.Employees
{
    public class EmployeeHierarchyDto
    {
        public int Id { get; set; }
        public int EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly? DateofBirth { get; set; }
        public int? DesignationId { get; set; }
        public string? DesignationName { get; set; }
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public bool? IsActive { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public EmploymentType? EmploymentType { get; set; }
        public string? Photo { get; set; }
        public List<EmployeeHierarchyDto> Subordinates { get; set; }

    }
}
