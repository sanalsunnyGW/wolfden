using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enum;

namespace WolfDen.Application.DTOs.LeaveManagement
{
    public class LeaveRequestDto
    {
        //public int EmployeeId { get; private set; }
        //public int TypeId { get; private set; }
        public bool HalfDay { get; private set; }
        public DateOnly FromDate { get; private set; }
        public DateOnly ToDate { get; private set; }
        public DateOnly ApplyDate { get; private set; }
        public LeaveRequestStatus LeaveRequestStatus { get; private set; }
        public string Description { get; private set; }
        public int ProcessedBy { get; private set; }
    }
}
