using MediatR;
using WolfDen.Application.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Attendence.DailyStatus
{
    public class DailyStatusQuery:IRequest<List<DailyStatusDTO>>
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
