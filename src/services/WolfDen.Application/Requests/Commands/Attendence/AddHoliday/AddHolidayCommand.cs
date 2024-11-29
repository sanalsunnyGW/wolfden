using MediatR;
using WolfDen.Domain.Enums;

namespace WolfDen.Application.Requests.Commands.Attendence.AddHoliday
{
    public class AddHolidayCommand:IRequest<int>
    {
        public DateOnly Date { get; set; }
        public AttendanceStatus Type { get; set; }
    }
}
