using MediatR;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Designations
{
    public class AddDesignationCommandHandler(WolfDenContext context) : IRequestHandler<AddDesignationCommand, int>
    {
        private readonly WolfDenContext _context = context;

        public async Task<int> Handle(AddDesignationCommand request, CancellationToken cancellationToken)
        {
            Designation designation = new Designation(request.DesignationName);
            _context.Designations.Add(designation);
            await _context.SaveChangesAsync(cancellationToken);
            return designation.Id;

        }
    }
}
