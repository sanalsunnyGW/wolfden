using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WolfDen.Application.DTOs.Employees
{
    public class GetAllEmployeePaginationResponseDTO
    {
        public List<EmployeeNameDTO> EmployeeNames {  get; set; }
        public int TotalRecords {  get; set; }
    }
}
