using MediatR;
using WolfDen.Application.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Attendence.GetHolidays
{
    public class GetHolidayQuery:IRequest<List<HolidayDTO>>
    {
        public int Year { get; set; }
    }
}
