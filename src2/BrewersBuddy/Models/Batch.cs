using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Security;

namespace BrewersBuddy.Models
{
    public class Batch
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BatchId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        [Required]
        public int BatchTypeValue { get; set; }
        //The user id of the user
        public int OwnerId { get; set; }

        public BatchType Type
        {
            get 
            {
                return (BatchType)Enum.ToObject(typeof(BatchType), BatchTypeValue); 
            }
            set
            {
                BatchTypeValue = (int)value;
            }
        }

        public virtual ICollection<Measurement> Measurements { get; set; }
        public virtual ICollection<BatchNote> Notes { get; set; }
        public virtual ICollection<BatchAction> Actions { get; set; }
        public virtual ICollection<UserProfile> Collaborators { get; set; }
        public virtual ICollection<BatchRating> Ratings { get; set; }
        public virtual ICollection<BatchComment> Comments { get; set; }
    }

}