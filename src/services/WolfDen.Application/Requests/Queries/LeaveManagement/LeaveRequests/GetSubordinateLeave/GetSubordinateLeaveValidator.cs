using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace WolfDen.Application.Requests.Queries.LeaveManagement.LeaveRequests.GetSubordinateLeave
{
    public class GetSubordinateLeaveValidator : AbstractValidator<GetSubordinateLeaveQuery>
    {
        public GetSubordinateLeaveValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Superior Required");
            RuleFor(x => x.StatusId).NotEmpty().WithMessage("Status Required");
        }
    }
}
