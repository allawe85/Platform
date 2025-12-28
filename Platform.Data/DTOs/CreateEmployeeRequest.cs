using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Data.DTOs
{
    public class CreateEmployeeRequest
    {
        public Employee Employee { get; set; }
        public List<LeaveBalance>? LeaveBalances { get; set; }
    }
}
