using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("poll_option")]
    public class PollOption
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("poll_id")]
        public int PollId { get; set; }

        [ForeignKey(nameof(PollId))]
        public Poll? Poll { get; set; }

        [Column("option_text")]
        [Required]
        [StringLength(500)]
        public string OptionText { get; set; }

        [Column("option_text_ar")]
        [Required]
        [StringLength(500)]
        public string OptionTextAr { get; set; }
    }
}
