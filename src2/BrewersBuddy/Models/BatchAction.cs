using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewersBuddy.Models
{
    [Table("BatchAction")]
    public class BatchAction
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ActionId { get; set; }

        [Required]
        public string Title { get; set; }

        [Display(Name = "Date")]
        public DateTime ActionDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int ActionTypeValue { get; set; }

        [Display(Name = "Performer")]
        public int PerformerId { get; set; }

        [Display(Name = "Batch")]
        public int BatchId { get; set; }

        public ActionType Type
        {
            get
            {
                return (ActionType)Enum.ToObject(typeof(ActionType), ActionTypeValue);
            }
            set
            {
                ActionTypeValue = (int)value;
            }
        }

        [ForeignKey("BatchId")]
        public virtual Batch Batch { get; set; }

        [ForeignKey("PerformerId")]
        public virtual UserProfile Performer { get; set; }

        public virtual String PerformerName
        {
            get
            {
                return this.Performer.FirstName + " " + this.Performer.LastName;
            }
        }

        public virtual String SummaryText
        {
            get
            {
                return this.Description.Length > 200 ? this.Description.Substring(0, 200) + "..." :
                       this.Description;
            }
        }
    }

}