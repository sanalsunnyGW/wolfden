using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeTeam;
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
            List<EmployeeHierarchyDto> employeeHierarchyDto = new List<EmployeeHierarchyDto>();
            GetEmployeeTeamQuery getEmployeeTeamQuery = new GetEmployeeTeamQuery
            {
                Id = request.Id,
                Hierarchy = true,
            };

            employeeHierarchyDto = await _mediator.Send(getEmployeeTeamQuery, cancellationToken);

            if (employeeHierarchyDto is null)
            {
                throw new Exception("Error");
            }

            List<int> listSubordinateEmployeeId = await GetAllChildIds(employeeHierarchyDto);
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


                   }).OrderByDescending(x => x.LeaveRequestId)
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



            async Task<List<int>>  GetAllChildIds(List<EmployeeHierarchyDto> employees)
            {
                List<int> result = new List<int>();

                foreach (EmployeeHierarchyDto employee in employees)
                {
                    if(employee.Id != request.Id)
                    result.Add(employee.Id);

                    if (employee.Subordinates != null && employee.Subordinates.Count > 0)
                    {
                        List<int> childIds = await GetAllChildIds(employee.Subordinates);
                        result.AddRange(childIds);
                    }
                }

                return  result;
            }

        }




    }
}
