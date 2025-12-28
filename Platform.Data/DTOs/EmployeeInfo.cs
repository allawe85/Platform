using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Data.DTOs
{
    [Table("view_employee_info")]
    public class EmployeeInfo
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
        
        [Column("full_name")]
        public string FullName { get; set; }

        [Column("UserName")]
        public string UserName { get; set; }
        
        [Column("Email")]
        public string Email { get; set; }
        
        [Column("PhoneNumber")]
        public string? PhoneNumber { get; set; }
        
        [Column("hierarchy_name")]
        public string HierarchyName { get; set; }
        
        [Column("hierarchy_name_ar")]
        public string HierarchyNameAr { get; set; }

        [Column("hierarchy_level_name")]
        public string HierarchyLevelName { get; set; }

        [Column("hierarchy_level_name_ar")]
        public string HierarchyLevelNameAr { get; set; }

    }
}
