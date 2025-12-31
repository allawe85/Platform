using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    public class Announcement
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [Column("title_ar")]
        public string TitleAr { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Column("description_ar")]
        public string? DescriptionAr { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }
    }
}
