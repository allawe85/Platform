using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("leave_balance")]
    public class LeaveBalance
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("employee_id")]
        public int? EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee Employee { get; set; }

        [Column("leave_type_id")]
        public int? LeaveTypeId { get; set; }

        [ForeignKey(nameof(LeaveTypeId))]
        public LeaveType LeaveType { get; set; }

        [Column("balance")]
        public int? Balance { get; set; }
    }
}
