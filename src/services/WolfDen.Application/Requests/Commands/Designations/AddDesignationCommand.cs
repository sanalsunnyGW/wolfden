using MediatR;

namespace WolfDen.Application.Requests.Commands.Designations
{
    public class AddDesignationCommand : IRequest<int>
    {
        public string DesignationName { get; set; }
    }
}
