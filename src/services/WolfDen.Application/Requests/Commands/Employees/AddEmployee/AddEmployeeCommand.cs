using MediatR;

namespace WolfDen.Application.Requests.Commands.Employees.AddEmployee
{
    public class AddEmployeecommand : IRequest<int>
    {
        public int EmployeeCode { get; set; }
        public string RFId { get; set; }
        public string FirstName { get; set; }


    }
  /*  public class Errors
    {
        public string[] _ { get; set; }
    }*/
}
