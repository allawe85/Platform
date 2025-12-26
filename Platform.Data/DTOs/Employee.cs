using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Data.DTOs
{
    [Table("employee")]
    public class Employee
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("civil_id")]
        public string CivilId { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [Column("hierarchy_id")]
        public int HierarchyId { get; set; }

        [Column("aspnetusers_id")]
        public string AspnetusersId { get; set; }
    }
}
