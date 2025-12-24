using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("poll")]
    public class Poll
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("question")]
        [Required]
        [StringLength(500)]
        public string Question { get; set; }

        [Column("question_ar")]
        [Required]
        [StringLength(500)]
        public string QuestionAr { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        public virtual ICollection<PollOption>? Options { get; set; }
    }
}
