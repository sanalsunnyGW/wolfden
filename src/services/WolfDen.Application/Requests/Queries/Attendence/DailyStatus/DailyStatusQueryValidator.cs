﻿using FluentValidation;

namespace WolfDen.Application.Requests.Queries.Attendence.DailyStatus
{
    public class DailyStatusQueryValidator : AbstractValidator<DailyStatusQuery>
    {
        public DailyStatusQueryValidator()
        {
            RuleFor(x => x.Year).NotEmpty().GreaterThan(0).WithMessage("Year must be greater than 0");
            RuleFor(x => x.Month).NotEmpty().GreaterThanOrEqualTo(1).LessThanOrEqualTo(12).WithMessage("Month must be between 1 to 12 ");
        }
    }
}
