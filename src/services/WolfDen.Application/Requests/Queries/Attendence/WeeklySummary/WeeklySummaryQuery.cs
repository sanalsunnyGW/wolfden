using MediatR;
using WolfDen.Application.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Attendence.WeeklySummary
{
    public class WeeklySummaryQuery:IRequest<List<WeeklySummaryDTO>>
    {
        public int EmployeeId { get; set; }
        public string WeekStart { get; set; }
        public string WeekEnd { get; set; }
    }
}
