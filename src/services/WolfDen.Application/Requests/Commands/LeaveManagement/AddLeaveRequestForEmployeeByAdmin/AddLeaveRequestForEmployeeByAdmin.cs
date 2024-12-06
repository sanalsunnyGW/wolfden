using MediatR;
using WolfDen.Application.DTOs.LeaveManagement;

namespace WolfDen.Application.Requests.Commands.LeaveManagement.AddLeaveRequestForEmployeeByAdmin
{
    public class AddLeaveRequestForEmployeeByAdmin : IRequest<ResponseDto>
    {
        public int AdminId { get; set; }
        public int EmployeeCode { get; set; }
        public int TypeId { get; set; }
        public bool? HalfDay {  get; set; }
        public DateOnly FromDate {  get; set; }
        public DateOnly ToDate { get; set; }
        public string Description { get; set; }
    }
}
