using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("Leave_Type")]
    public class LeaveType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name_ar")]
        [StringLength(200)]
        public string NameAr { get; set; }

        [Column("type_balance")]
        public int? TypeBalance { get; set; }
    }
}
