using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Application.DTOs.Attendence
{
    public class AttendanceSummaryDTO
    {
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Late { get; set; }
        public int WFH { get; set; }
    }
}
