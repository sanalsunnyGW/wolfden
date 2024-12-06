﻿using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.LeaveRequests.AddLeaveRequest
{
    public class AddLeaveRequestCommand : IRequest<ResponseDto>
    {
        public int EmpId { get; set; }
        public int TypeId { get; set; }
        public bool? HalfDay { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string Description { get; set; }

    }
}
