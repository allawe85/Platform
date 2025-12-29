using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("Leave_Status")]
    public class LeaveStatus
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name_ar")]
        [StringLength(10)]
        public string NameAr { get; set; }

        [Column("name")]
        [StringLength(10)]
        public string? Name { get; set; }
    }
}
