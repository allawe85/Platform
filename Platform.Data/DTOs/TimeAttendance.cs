using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("time_attendance")]
    public class TimeAttendance
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [Column("transaction_time")]
        public DateTime TransactionTime { get; set; }

        [Column("transaction_type")]
        [StringLength(5)]
        public string TransactionType { get; set; } // "IN" or "OUT"
    }
}
