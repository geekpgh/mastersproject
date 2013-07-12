using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BrewersBuddy.Models
{
    [Table("BatchComment")]
    public class BatchComment
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BatchCommentId { get; set; }

        [Required]
        public int BatchId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Comment { get; set; }

        [ForeignKey("BatchId")]
        public virtual Batch Batch { get; set; }

        [ForeignKey("UserId")]
        public virtual UserProfile User { get; set; }
    }
}