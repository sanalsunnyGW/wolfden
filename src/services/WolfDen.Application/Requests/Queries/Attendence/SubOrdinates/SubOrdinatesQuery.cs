using MediatR;
using WolfDen.Application.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Attendence.SubOrdinates
{
    public class SubOrdinatesQuery:IRequest<SubOrdinateDTO>
    {
        public int EmployeeId { get; set; }
    }
}
