using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Application.DTOs.Attendence
{
    public class AttendanceHistoryDTO
    {
        public List<WeeklySummaryDTO>? AttendanceHistory { get; set; }
        public int TotalPages { get; set; }
    }
}
