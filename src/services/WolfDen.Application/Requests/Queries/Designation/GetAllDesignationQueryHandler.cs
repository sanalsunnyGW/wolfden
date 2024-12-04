using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Application.DTOs.Employees;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Designation
{
    public class GetAllDesignationQueryHandler: IRequestHandler<GetAllDesignationQuery, List<DesignationDTO>>
    {
        private readonly WolfDenContext _context;

        public GetAllDesignationQueryHandler(WolfDenContext context)
        {
            _context = context;
        }

        public async Task<List<DesignationDTO>> Handle(GetAllDesignationQuery query, CancellationToken cancellationToken)
        {
            List<DesignationDTO> designation = await _context.Designations
              .Select(designation => new DesignationDTO
              {
                  Id = designation.Id,
                  DesignationName = designation.Name
              })
              .ToListAsync(cancellationToken);

            return designation;

        }

    }
}
