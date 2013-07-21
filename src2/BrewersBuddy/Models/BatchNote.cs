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
        
        [Display(Name = "Date")]
        public DateTime AuthorDate { get; set; }
        
        public string Text { get; set; }

        [Display(Name = "Batch")]
        public int BatchId { get; set; }

        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        [ForeignKey("BatchId")]
        public virtual Batch Batch { get; set; }

        [ForeignKey("AuthorId")]
        public virtual UserProfile Author { get; set;  }

        public virtual String AuthorName
        {
            get
            {
                return this.Author.FirstName + " " + this.Author.LastName;
            }
        }

        public virtual String SummaryText
        {
            get
            {
                return this.Text.Length > 200 ? this.Text.Substring(0, 200) + "..." : 
                       this.Text;
            }
        }

        public bool CanView(int userId)
        {
            Batch batch = this.Batch;
            return batch.CanView(userId);
        }


        public bool CanEdit(int userId)
        {
            Batch batch = this.Batch;
            return batch.CanEdit(userId);
        }
    }

}