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
        public string Name { get; set; } = string.Empty;

        [Column("name_ar")]
        public string NameAr { get; set; } = string.Empty;

        [Column("location")]
        public string Location { get; set; } = string.Empty;

        [Column("location_ar")]
        public string LocationAr { get; set; } = string.Empty;

        [Column("event_type_id")]
        public int EventTypeId { get; set; }

        public virtual EventType? EventType { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("description_ar")]
        public string DescriptionAr { get; set; } = string.Empty;
    }
}
