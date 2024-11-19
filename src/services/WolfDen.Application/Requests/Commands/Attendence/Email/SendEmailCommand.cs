using MediatR;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;

namespace WolfDen.Application.Requests.Commands.Attendence.Email
{
    public class SendEmailCommand:IRequest<string>
    {
        public int EmployeeId { get; set;}
    }
}
