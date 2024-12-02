using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeTeam;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetSubordinateLeave
{
    public class GetSubordinateLeaveQueryHandler(WolfDenContext context, IMediator mediator) : IRequestHandler<GetSubordinateLeaveQuery, SubordinateLeaveRequestPaginationDto>
    {
        private readonly WolfDenContext _context = context;
        private readonly IMediator _mediator = mediator;
        public async Task<SubordinateLeaveRequestPaginationDto> Handle(GetSubordinateLeaveQuery request, CancellationToken cancellationToken)
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
            List<SubordinateLeaveDto> subordinateLeaveDtosList = new List<SubordinateLeaveDto>();
            SubordinateLeaveRequestPaginationDto subordinateLeaveRequestPaginationDto = new SubordinateLeaveRequestPaginationDto();

            if (request.StatusId == LeaveRequestStatus.Rejected)
            {
                subordinateLeaveDtosList = await _context.LeaveRequests
                   .Where(x => listSubordinateEmployeeId.Contains(x.EmployeeId) && x.LeaveRequestStatusId == LeaveRequestStatus.Rejected && x.ProcessedBy == request.Id)
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
                   .Skip((request.PageNumber) * request.PageSize)
                   .Take(request.PageSize)
                   .ToListAsync(cancellationToken);
                subordinateLeaveRequestPaginationDto.SubordinateLeaveDtosList = subordinateLeaveDtosList;
                subordinateLeaveRequestPaginationDto.TotalDataCount = await _context.LeaveRequests
                   .Where(x => listSubordinateEmployeeId.Contains(x.EmployeeId) && x.LeaveRequestStatusId == LeaveRequestStatus.Rejected && x.ProcessedBy == request.Id).CountAsync();

            }
            else if (request.StatusId == LeaveRequestStatus.Approved)
            {
                subordinateLeaveDtosList = await _context.LeaveRequests
                   .Where(x => listSubordinateEmployeeId.Contains(x.EmployeeId) && x.LeaveRequestStatusId == LeaveRequestStatus.Approved && x.ProcessedBy == request.Id)
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
                   .Skip((request.PageNumber) * request.PageSize)
                   .Take(request.PageSize)
                   .ToListAsync(cancellationToken);
                subordinateLeaveRequestPaginationDto.SubordinateLeaveDtosList = subordinateLeaveDtosList;
                subordinateLeaveRequestPaginationDto.TotalDataCount = await _context.LeaveRequests
                   .Where(x => listSubordinateEmployeeId.Contains(x.EmployeeId) && x.LeaveRequestStatusId == LeaveRequestStatus.Approved && x.ProcessedBy == request.Id).CountAsync();
            }

            else
            {
                subordinateLeaveDtosList = await _context.LeaveRequests
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
                   .Skip((request.PageNumber) * request.PageSize)
                   .Take(request.PageSize)
                   .ToListAsync(cancellationToken);
                subordinateLeaveRequestPaginationDto.SubordinateLeaveDtosList = subordinateLeaveDtosList;
                subordinateLeaveRequestPaginationDto.TotalDataCount = await _context.LeaveRequests
                   .Where(x => listSubordinateEmployeeId.Contains(x.EmployeeId) && x.LeaveRequestStatusId == LeaveRequestStatus.Open ).CountAsync();
            }

            return subordinateLeaveRequestPaginationDto;



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
