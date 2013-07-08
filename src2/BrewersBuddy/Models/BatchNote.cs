using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewersBuddy.Models
{
    [Table("BatchNote")]
    public class BatchNote
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }
        public string Title { get; set; }
        public DateTime AuthorDate { get; set; }
        public string Text { get; set; }
        public int BatchId { get; set; }
        public int AuthorId { get; set; }

        [ForeignKey("BatchId")]
        public virtual Batch Batch { get; set; }
    }

}