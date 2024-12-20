﻿using MediatR;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.EditLeaveRequest
{
    public class EditLeaveRequestCommand : IRequest<bool>
    {
        public int EmpId { get; set; }  
        public int LeaveRequestId { get; set; }
        public int TypeId { get; set; }
        public bool? HalfDay { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string Description { get; set; }

    }
}
