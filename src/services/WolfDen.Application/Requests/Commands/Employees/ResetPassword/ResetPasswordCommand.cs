using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Application.Requests.Commands.Employees.ResetPassword
{
    public class ResetPasswordCommand: IRequest<bool>
    {
        public int Id { get; set; }
        public string password {  get; set; }
    }
}
