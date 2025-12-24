using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("event")]
    public class Event
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Column("name_ar")]
        [Required]
        [MaxLength(200)]
        public string NameAr { get; set; } = string.Empty;

        [Column("location")]
        [Required]
        [MaxLength(500)]
        public string Location { get; set; } = string.Empty;

        [Column("location_ar")]
        [Required]
        [MaxLength(500)]
        public string LocationAr { get; set; } = string.Empty;

        [Column("event_type_id")]
        public int EventTypeId { get; set; }

        [ForeignKey(nameof(EventTypeId))]
        public virtual EventType? EventType { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("description")]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Column("description_ar")]
        [MaxLength(500)]
        public string DescriptionAr { get; set; } = string.Empty;
    }
}
