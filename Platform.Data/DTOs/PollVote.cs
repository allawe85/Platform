using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Data.DTOs
{
    [Table("poll_vote")]
    public class PollVote
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("poll_id")]
        public int PollId { get; set; }

        [ForeignKey(nameof(PollId))]
        public Poll? Poll { get; set; }

        [Column("poll_option_id")]
        public int PollOptionId { get; set; }

        [ForeignKey(nameof(PollOptionId))]
        public PollOption? PollOption { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        [Column("voted_at")]
        public DateTime VotedAt { get; set; }
    }
}
