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
        public string Title { get; set; }
        public DateTime ActionDate { get; set; }
        public string Description { get; set; }
        [Required]
        public int ActionTypeValue { get; set; }
        public Batch Batch { get; set; }
        public int PerformerId { get; set; }

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
    }

}