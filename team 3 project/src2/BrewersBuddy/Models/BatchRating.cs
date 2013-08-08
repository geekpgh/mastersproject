using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewersBuddy.Models
{
    [Table("BatchRating")]
    public class BatchRating
    {
        [Key, Column(Order = 0)]
        [Required]
        public int BatchId { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        public int UserId { get; set; }

        [Range(0, 100)]
        public int Rating { get; set; }

        [MaxLength(Int32.MaxValue)]
        public string Comment { get; set; }

        [ForeignKey("BatchId")]
        public virtual Batch Batch { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
    }
}