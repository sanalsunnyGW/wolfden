using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.DesignationCommands
{
    public class AddDesignationCommandHandler : IRequestHandler<AddDesignationCommand, int>
    {
        private readonly WolfDenContext _context;

        public AddDesignationCommandHandler(WolfDenContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddDesignationCommand request, CancellationToken cancellationToken)
        {
            Designation designation = new Designation(request.DesignationName);
            _context.Designations.Add(designation);
            return await _context.SaveChangesAsync();

        }
    }
}
