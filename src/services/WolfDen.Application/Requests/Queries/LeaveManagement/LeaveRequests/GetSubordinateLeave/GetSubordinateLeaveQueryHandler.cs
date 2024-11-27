using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeHierarchy;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetSubordinateLeave
{
    public class GetSubordinateLeaveQueryHandler(WolfDenContext context, IMediator mediator) : IRequestHandler<GetSubordinateLeaveQuery, List<SubordinateLeaveDto>>
    {
        private readonly WolfDenContext _context = context;
        private readonly IMediator _mediator = mediator;
        public async Task<List<SubordinateLeaveDto>> Handle(GetSubordinateLeaveQuery request, CancellationToken cancellationToken)
        {
            EmployeeHierarchyDto employeeHierarchyDto = new EmployeeHierarchyDto();
            GetEmployeeHierarchyQuery getEmployeeHierarchyQuery = new GetEmployeeHierarchyQuery
            {
                Id = request.Id,
            };

            employeeHierarchyDto = await _mediator.Send(getEmployeeHierarchyQuery, cancellationToken);

            if (employeeHierarchyDto == null)
            {
                throw new Exception("Error");
            }

            List<int> listSubordinateEmployeeId = GetAllChildIds(employeeHierarchyDto);
            List<SubordinateLeaveDto> subordinateLeaveDtos;

            if (request.StatusId == LeaveRequestStatus.Rejected)
            {
                subordinateLeaveDtos = await _context.LeaveRequests
                   .Where(x => listSubordinateEmployeeId.Contains(x.EmployeeId) && x.LeaveRequestStatusId == LeaveRequestStatus.Rejected)
                   .Select(x => new SubordinateLeaveDto
                   {
                       LeaveRequestId = x.Id,
                       Name = x.Employee.FirstName + " " + x.Employee.LastName,
                       EmployeeCode = x.Employee.EmployeeCode,
                       TypeName = x.LeaveType.TypeName,
                       HalfDay = x.HalfDay,
                       ApplyDate = x.ApplyDate,
                       FromDate = x.FromDate,
                       ToDate = x.ToDate,
                       Description = x.Description,


                   }).OrderByDescending(x => x.LeaveRequestId)
                   .ToListAsync(cancellationToken);
            }
            else if (request.StatusId == LeaveRequestStatus.Approved)
            {
                subordinateLeaveDtos = await _context.LeaveRequests
                   .Where(x => listSubordinateEmployeeId.Contains(x.EmployeeId) && x.LeaveRequestStatusId == LeaveRequestStatus.Approved)
                   .Select(x => new SubordinateLeaveDto
                   {
                       LeaveRequestId = x.Id,
                       Name = x.Employee.FirstName + " " + x.Employee.LastName,
                       EmployeeCode = x.Employee.EmployeeCode,
                       TypeName = x.LeaveType.TypeName,
                       HalfDay = x.HalfDay,
                       ApplyDate = x.ApplyDate,
                       FromDate = x.FromDate,
                       ToDate = x.ToDate,
                       Description = x.Description,


                   }).OrderByDescending (x => x.LeaveRequestId)
                   .ToListAsync(cancellationToken);
            }

            else
            {
                subordinateLeaveDtos = await _context.LeaveRequests
                   .Where(x => listSubordinateEmployeeId.Contains(x.EmployeeId) && x.LeaveRequestStatusId == LeaveRequestStatus.Open)
                   .Select(x => new SubordinateLeaveDto
                   {
                       LeaveRequestId = x.Id,
                       Name = x.Employee.FirstName + " " + x.Employee.LastName,
                       EmployeeCode = x.Employee.EmployeeCode,
                       TypeName = x.LeaveType.TypeName,
                       HalfDay = x.HalfDay,
                       ApplyDate = x.ApplyDate,
                       FromDate = x.FromDate,
                       ToDate = x.ToDate,
                       Description = x.Description,


                   }).OrderByDescending(x => x.LeaveRequestId)
                   .ToListAsync(cancellationToken);
            }

            return subordinateLeaveDtos;



        }

        private List<int> GetAllChildIds(EmployeeHierarchyDto employee)
        {
            List<int> result = new List<int>();
            if (employee.Subordinates == null || employee.Subordinates.Count == 0)
            {
                return result; 
            }

            foreach (EmployeeHierarchyDto subordinate in employee.Subordinates)
            {
                result.Add(subordinate.Id); 
                result.AddRange(GetAllChildIds(subordinate));
            }

            return result;
        }


    }
}
