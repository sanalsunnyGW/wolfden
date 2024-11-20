using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Domain.Entity;

namespace WolfDen.Application.DTOs.LeaveManagement
{
    public class LeaveIncrementLogDto
    {
        public int LeaveBalanceId { get; set; }
        public DateOnly LogDate { get; set; }
        public decimal CurrentBalance { get; set; }
        public int IncrementValue { get;  set; }
        public decimal LeaveBalance { get; set; }
    }
}
