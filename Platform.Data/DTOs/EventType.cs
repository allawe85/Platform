using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("event_type")]
    public class EventType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("name_ar")]
        public string NameAr { get; set; } = string.Empty;
    }
}
