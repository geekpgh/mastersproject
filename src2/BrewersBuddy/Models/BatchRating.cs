using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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

        [Range(0,100)]
        public int Rating { get; set; }
        [MaxLength(Int32.MaxValue)]
        public string Comment { get; set; }

        public virtual Batch Batch { get; set; }
        public virtual UserProfile User { get; set; }
    }
}