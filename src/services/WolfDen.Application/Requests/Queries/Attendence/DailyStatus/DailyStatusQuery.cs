using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using WolfDen.Application.Requests.DTOs.Attendence;

namespace WolfDen.Application.Requests.Queries.Attendence.DailyStatus
{
    public class DailyStatusQuery:IRequest<List<DailyStatusDTO>>
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
