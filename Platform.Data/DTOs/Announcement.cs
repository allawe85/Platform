using System;
using System.ComponentModel.DataAnnotations;

namespace Platform.Data.DTOs
{
    public class Announcement
    {
        [Key]
        public int AnnouncementId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
