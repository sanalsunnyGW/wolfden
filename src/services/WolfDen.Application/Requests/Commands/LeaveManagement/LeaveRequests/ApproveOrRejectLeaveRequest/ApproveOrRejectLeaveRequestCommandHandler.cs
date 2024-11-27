using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Application.Requests.Queries.Employees.GetEmployeeHierarchy;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.ApproveOrRejectLeaveRequest
{
    public class ApproveOrRejectLeaveRequestCommandHandler(WolfDenContext context, IMediator mediator) : IRequestHandler<ApproveOrRejectLeaveRequestCommand, bool>
    {
        private readonly WolfDenContext _context = context;
        private readonly IMediator _mediator = mediator;
        public async Task<bool> Handle(ApproveOrRejectLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            LeaveRequest leaveRequest = await _context.LeaveRequests.Where(x => x.Id == request.LeaveRequestId && x.LeaveRequestStatusId == LeaveRequestStatus.Open).FirstOrDefaultAsync(cancellationToken);
            LeaveType leaveType1 = await _context.LeaveType.Where(x => x.Id == leaveRequest.TypeId).FirstOrDefaultAsync(cancellationToken);  
            EmployeeHierarchyDto employeeHierarchyDto = new EmployeeHierarchyDto();
            GetEmployeeHierarchyQuery getEmployeeHierarchyQuery = new GetEmployeeHierarchyQuery
            {
                Id = request.SuperiorId,
            };

            employeeHierarchyDto = await _mediator.Send(getEmployeeHierarchyQuery, cancellationToken);

            if (employeeHierarchyDto == null)
            {
                throw new Exception("No Subordinates");
            }

            List<int> listSubordinateEmployeeId = GetAllChildIds(employeeHierarchyDto);

            if (listSubordinateEmployeeId.Contains(leaveRequest.EmployeeId))
            {
                if (request.statusId == LeaveRequestStatus.Rejected )
                {
                    leaveRequest.Reject();
                    int result =  await _context.SaveChangesAsync(cancellationToken);
                    return result > 0;
                }
                else if (leaveType1.LeaveCategoryId != null && (request.statusId == LeaveRequestStatus.Approved && leaveRequest.LeaveType.LeaveCategoryId != LeaveCategory.WorkFromHome))
                    {
                    leaveRequest.Approve();
                    LeaveBalance leaveBalance = await _context.LeaveBalances.FirstOrDefaultAsync(x => x.TypeId == leaveRequest.TypeId && x.EmployeeId == leaveRequest.EmployeeId);

                    decimal daysCount = await _context.LeaveRequestDays.Where(x => x.LeaveRequestId == request.LeaveRequestId).CountAsync();

                    if (leaveRequest.HalfDay == true)
                    {
                        daysCount = daysCount - 0.5m;

                    }

                    if (leaveType1.LeaveCategoryId != null && ( leaveRequest.ApplyDate >= leaveRequest.FromDate && leaveRequest.LeaveType.LeaveCategoryId !=LeaveCategory.BereavementLeave))
                    {
                        LeaveType leaveType = await _context.LeaveType.FirstOrDefaultAsync(x => x.LeaveCategoryId == LeaveCategory.EmergencyLeave);
                        LeaveBalance leaveBalance2 = await _context.LeaveBalances.FirstOrDefaultAsync(x => x.TypeId == leaveType.Id && x.EmployeeId == leaveRequest.EmployeeId);


                        leaveBalance2.Balance = leaveBalance2.Balance - daysCount;
                        leaveBalance2.UpdateBalance(leaveBalance2.Balance);
                    }

                    leaveBalance.Balance = leaveBalance.Balance - daysCount;
                    leaveBalance.UpdateBalance(leaveBalance.Balance);
                    int result = await _context.SaveChangesAsync(cancellationToken);
                    return result > 0;
                }
                else if(leaveType1.LeaveCategoryId != null && ( leaveRequest.LeaveType.LeaveCategoryId == LeaveCategory.WorkFromHome && request.statusId == LeaveRequestStatus.Approved))  
                {
                    leaveRequest.Approve();
                    int result = await _context.SaveChangesAsync(cancellationToken);
                    return result > 0;
                }
                else
                {
                    throw new Exception($"Wrong Approval Type or Leave category of Leave Type has no value");
                }

            }
            else
            {
                throw new Exception("You are not superior to the applied leave request employee");
            }


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
