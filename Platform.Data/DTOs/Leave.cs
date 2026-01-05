using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("Leave")]
    public class Leave
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("leave_type_id")]
        public int? LeaveTypeId { get; set; }

        [ForeignKey(nameof(LeaveTypeId))]
        public LeaveType? LeaveType { get; set; }

        [Column("employee_id")]
        public int? EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        [Column("leave_status_id")]
        public int? LeaveStatusId { get; set; }

        [ForeignKey(nameof(LeaveStatusId))]
        public LeaveStatus? LeaveStatus { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("details")]
        [StringLength(500)]
        public string Details { get; set; }
    }
}
