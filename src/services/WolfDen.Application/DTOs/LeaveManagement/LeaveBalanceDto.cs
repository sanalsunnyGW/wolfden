using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Domain.Entity;

namespace WolfDen.Application.DTOs.LeaveManagement
{
    public class LeaveBalanceDto
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }
}
