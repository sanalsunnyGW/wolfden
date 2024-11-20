 using MediatR;
using Microsoft.EntityFrameworkCore;
using WolfDen.Application.DTOs.LeaveManagement;
using WolfDen.Application.Requests.Queries.LeaveManagement.LeaveTypes.WolfDen.Application.Requests.Queries.LeaveManagement.LeaveTypes;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveTypes
    {
        public class GetAllLeaveTypeIdAndNameQueryHandler(WolfDenContext context) : IRequestHandler<GetAllLeaveTypeIdAndNameQuery, List<LeaveTypeDto>>
        {
            private readonly WolfDenContext _context = context;

            public async Task<List<LeaveTypeDto>> Handle(GetAllLeaveTypeIdAndNameQuery request, CancellationToken cancellationToken)
            {
                List<LeaveTypeDto> leaveTypeDtoList = new List<LeaveTypeDto>();
                List<LeaveType> leaveTypesList = new List<LeaveType>();
                leaveTypesList = await _context.LeaveType.ToListAsync(cancellationToken).ConfigureAwait(false);

                if (leaveTypesList == null)
                {
                    throw new KeyNotFoundException("LeaveType records not found.");
                }

                foreach (LeaveType leaveType in leaveTypesList)
                {
                    LeaveTypeDto leaveTypeDto = new LeaveTypeDto();
                    leaveTypeDto.Id = leaveType.Id;
                    leaveTypeDto.Name = leaveType.TypeName;
                    leaveTypeDtoList.Add(leaveTypeDto);
                }

                return leaveTypeDtoList;
            }
        }
}


