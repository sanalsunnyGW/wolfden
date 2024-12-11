using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeHierarchy;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeTeam;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetApprovedNextWeekLeaves;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetNextWeekApprovedLeaveRequests
{
    public class GetNextWeekApprovedLeaveQueryHandler(WolfDenContext context, IMediator mediator) : IRequestHandler<GetNextWeekApprovedLeaveQuery, List<LeaveRequestDto>>
    {
        private readonly WolfDenContext _context = context;
        private readonly IMediator _mediator = mediator;
        public async Task<List<LeaveRequestDto>> Handle(GetNextWeekApprovedLeaveQuery request, CancellationToken cancellationToken)
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            int daysUntilNextWeek = ((int)DayOfWeek.Monday - (int)currentDate.DayOfWeek + 7) % 7;
            DateOnly startOfNextWeek = currentDate.AddDays(daysUntilNextWeek);
            DateOnly[] nextWeekDates = new DateOnly[7];
            List<LeaveRequestDto> LeaveRequestDtoList = new List<LeaveRequestDto>();
            for (int i = 0; i < 5; i++)
            {
                nextWeekDates[i] = startOfNextWeek.AddDays(i);
            }


            if (request.EmployeeId != null)
            {
                List<Employee> employee = await _context.Employees.Where(x => x.ManagerId.Equals(request.EmployeeId)).ToListAsync(cancellationToken);
                GetEmployeeTeamQuery getEmployeeTeam = new GetEmployeeTeamQuery();

                if (employee.Any())
                {
                    getEmployeeTeam.Id = (int)request.EmployeeId;
                    getEmployeeTeam.Hierarchy = true;
                }
                else
                {
                    getEmployeeTeam.Id = (int)request.EmployeeId;
                    getEmployeeTeam.Hierarchy = false;
                }



                List<EmployeeHierarchyDto> employeeHierarchy = new List<EmployeeHierarchyDto>();
                employeeHierarchy = await _mediator.Send(getEmployeeTeam, cancellationToken);      //list of my subordinates
                List<int> subEmployeesIds = await GetAllChildIds(employeeHierarchy);

                LeaveRequestDtoList = await _context.LeaveDays
                                                                  .Where(x => x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Approved
                                                                              && subEmployeesIds.Contains(x.LeaveRequest.EmployeeId)
                                                                              && nextWeekDates.Contains(x.LeaveDate))
                                                            .GroupBy(x => x.LeaveRequestId)
                                                            .Select(group => new LeaveRequestDto
                                                            {
                                                                Id = group.Key,
                                                                Name = group.FirstOrDefault().LeaveRequest.Employee.FirstName,
                                                                FromDate = group.FirstOrDefault().LeaveRequest.FromDate,
                                                                ToDate = group.FirstOrDefault().LeaveRequest.ToDate,
                                                                ApplyDate = group.FirstOrDefault().LeaveRequest.ApplyDate,
                                                                TypeName = group.FirstOrDefault().LeaveRequest.LeaveType.TypeName,
                                                                HalfDay = group.FirstOrDefault().LeaveRequest.HalfDay,
                                                                ProcessedBy = group.FirstOrDefault().LeaveRequest.Employee.FirstName
                                                            })
                                                            .ToListAsync(cancellationToken);
            }
            else
            {

                GetEmployeeHierarchyQuery getCompanyEmployees = new GetEmployeeHierarchyQuery();
                EmployeeHierarchyDto companyHierarchy = new EmployeeHierarchyDto();

                companyHierarchy = await _mediator.Send(getCompanyEmployees, cancellationToken);      //list of my subordinates
                List<int> allEmployeesIds = await GetAllEmployeeIds(companyHierarchy);

                LeaveRequestDtoList = await _context.LeaveDays
                                                                  .Where(x => x.LeaveRequest.LeaveRequestStatusId == LeaveRequestStatus.Approved
                                                                         && allEmployeesIds.Contains(x.LeaveRequest.EmployeeId)
                                                                              && nextWeekDates.Contains(x.LeaveDate))
                                                            .GroupBy(x => x.LeaveRequestId)
                                                            .Select(group => new LeaveRequestDto
                                                            {
                                                                Id = group.Key,
                                                                Name = group.FirstOrDefault().LeaveRequest.Employee.FirstName,
                                                                FromDate = group.FirstOrDefault().LeaveRequest.FromDate,
                                                                ToDate = group.FirstOrDefault().LeaveRequest.ToDate,
                                                                ApplyDate = group.FirstOrDefault().LeaveRequest.ApplyDate,
                                                                TypeName = group.FirstOrDefault().LeaveRequest.LeaveType.TypeName,
                                                                HalfDay = group.FirstOrDefault().LeaveRequest.HalfDay,
                                                                ProcessedBy = group.FirstOrDefault().LeaveRequest.Employee.FirstName
                                                            })
                                                            .ToListAsync(cancellationToken);

            }

            async Task<List<int>> GetAllChildIds(List<EmployeeHierarchyDto> employees)
            {
                List<int> result = new List<int>();

                foreach (EmployeeHierarchyDto employee in employees)
                {
                    result.Add(employee.Id);
                    if (employee.Subordinates is not null)
                    {
                        List<int> childIds = await GetAllChildIds(employee.Subordinates);
                        result.AddRange(childIds);
                    }
                    else
                    {
                        List<int> childIds = await GetAllEmployeeIds(employee);
                    }
                }
                return result;
            }

            async Task<List<int>> GetAllEmployeeIds(EmployeeHierarchyDto employee)
            {
                List<int> result = new List<int>();
                if (employee.Subordinates is not null)
                {
                    foreach (EmployeeHierarchyDto subOrdinate in employee.Subordinates)
                    {
                        result.Add(subOrdinate.Id);
                        List<int> childIds = await GetAllEmployeeIds(subOrdinate);
                        result.AddRange(childIds);
                    }
                }
                return result;
            }

            return LeaveRequestDtoList;
        }
    }
}


