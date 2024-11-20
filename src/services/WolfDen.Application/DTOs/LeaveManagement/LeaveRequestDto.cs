using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Domain.Entity;
using WolfDen.Domain.Enums;

namespace WolfDen.Application.DTOs.LeaveManagement
{
    public class LeaveRequestDto
    {
       
        public string TypeName { get; set; }  //to get leave type name from table LeaveType
        public bool HalfDay { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public DateOnly ApplyDate { get;set; }
        public LeaveRequestStatus LeaveRequestStatus { get; set; }
        public string Description { get; set; }
        public string ProcessedBy { get; set; }
    }
}
