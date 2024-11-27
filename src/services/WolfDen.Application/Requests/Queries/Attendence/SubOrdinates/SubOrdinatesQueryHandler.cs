using MediatR;
using WolfDen.Application.DTOs.Attendence;
using WolfDen.Application.Requests.Queries.Attendence.SubOrdinates;
using WolfDen.Application.Requests.Services;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Queries.Employees.SubOrdinates
{
    public class SubOrdinatesQueryHandler(WolfDenContext context) : IRequestHandler<SubOrdinatesQuery,SubOrdinateDTO>
    {
        private readonly WolfDenContext _context = context;

        public async Task<SubOrdinateDTO> Handle(SubOrdinatesQuery request, CancellationToken cancellationToken)
        {
            TeamHeirarchyService service = new(_context);
            Employee employee = await _context.Employees.FindAsync(request.EmployeeId, cancellationToken);
            if (employee is null) 
            {
                return null;
            }
            SubOrdinateDTO subOrdinate = new SubOrdinateDTO();
            if (employee.IsActive == false)
            {
                return null;
            }
            subOrdinate.SubOrdinates = await service.GetSubordinates(employee.Id);
            return subOrdinate;
        }
    }
}




