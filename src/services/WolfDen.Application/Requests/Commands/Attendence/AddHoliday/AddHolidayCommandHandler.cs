using MediatR;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Data;

namespace WolfDen.Application.Requests.Commands.Attendence.AddHoliday
{
    public class AddHolidayCommandHandler(WolfDenContext context) : IRequestHandler<AddHolidayCommand, int>
    {
        private readonly WolfDenContext _context=context;
        public async Task<int> Handle(AddHolidayCommand request, CancellationToken cancellationToken)
        {
            Holiday holiday = new Holiday(request.Date, request.Type);

            await _context.Holiday.AddAsync(holiday);
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
