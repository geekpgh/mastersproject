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

        public DateTime ActionDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int ActionTypeValue { get; set; }

        public int PerformerId { get; set; }

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
    }

}