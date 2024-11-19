using WolfDen.Domain.Enums;

namespace WolfDen.Domain.Entity
{
    public class LeaveRequest
    {
        public int Id { get; private set; }
        public int EmployeeId { get; private set; }
        public int TypeId { get; private set; }
        public bool HalfDay { get; private set; }
        public DateOnly FromDate { get; private set; }
        public DateOnly ToDate { get; private set; }
        public DateOnly ApplyDate { get; private set; }
        public LeaveRequestStatus LeaveRequestStatusId { get; private set; }
        public string Description { get; private set; }
        public int ProcessedBy { get; private set; }
        public virtual Employee Employee { get; private set; }
        public virtual LeaveType LeaveType { get; private set; }
        public virtual Employee Manager { get; private set; }
        private LeaveRequest() { }

        public LeaveRequest(int employeeId, int typeId, bool halfDay, DateOnly fromDate, DateOnly toDate, DateOnly applyDate, LeaveRequestStatus leaveRequestStatusId, string description, int processedBy)
        {
            EmployeeId = employeeId;
            TypeId = typeId;
            HalfDay = halfDay;
            FromDate = fromDate;
            ToDate = toDate;
            ApplyDate = applyDate;
            LeaveRequestStatusId = leaveRequestStatusId;
            Description = description;
            ProcessedBy = processedBy;

        }

        public void GetLeaveRequestHistory()
        {

        }
    }
}
