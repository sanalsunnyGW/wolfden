using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WolfDen.Domain.Entity;

namespace WolfDen.Application.Validators.LeaveManagement
{
    public class LeaveTypeValidator : AbstractValidator<LeaveType>
    {
        public LeaveTypeValidator()
        {
            RuleFor(x => x.TypeName).NotEmpty().WithMessage("Type Name Required");
            RuleFor(x => x.HalfDay).NotEmpty().WithMessage("Choose Half Day or Full Day");
            RuleFor(x => x.Hidden).NotEmpty().WithMessage("Choose if Hidden or Not");
            RuleFor(x => x.RestrictionType).NotEmpty().WithMessage("Choose Restricted Type or Normal Type");
        }
    }
}
