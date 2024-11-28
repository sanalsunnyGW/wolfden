using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs.Employees;

namespace WolfDen.Application.Requests.Queries.Designation
{
    public class GetAllDesignationQuery : IRequest<List<DesignationDTO>>
    {
        public GetAllDesignationQuery()
        {

        }
    }
}
