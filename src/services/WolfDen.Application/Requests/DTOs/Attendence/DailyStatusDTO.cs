﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Application.Requests.DTOs.Attendence
{
    public class DailyStatusDTO
    {
        public DateOnly Date { get; set; }
        public string Status { get; set; }
    }
}