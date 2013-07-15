using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Security;
using System.Linq;

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
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        public int BatchTypeValue { get; set; }
        //The user id of the user
        [Display(Name = "Owner")]
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

        public virtual String SummaryText
        {
            get
            {
                return this.Description.Length > 200 ? this.Description.Substring(0, 200) + "..." :
                       this.Description;
            }
        }

        [ForeignKey("OwnerId")]
        public virtual UserProfile Owner { get; set; }
        public virtual ICollection<Measurement> Measurements { get; set; }
        public virtual ICollection<BatchNote> Notes { get; set; }
        public virtual ICollection<BatchAction> Actions { get; set; }
        public virtual ICollection<UserProfile> Collaborators { get; set; }
        public virtual ICollection<BatchRating> Ratings { get; set; }
        public virtual ICollection<BatchComment> Comments { get; set; }

        /// <summary>
        /// Determines whether this instance can be rated by the user with the
        /// specified id
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>
        ///   <c>true</c> if this instance can can be rated by the user with 
        ///   the specified id; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRate(int userId)
        {
            bool hasRated = HasRated(userId);

            if (OwnerId == userId)
            {
                if (!hasRated)
                    return true;
                else
                    return false;
            }

            if (Collaborators != null)
            {
                IEnumerable<int> collaboratorIds = Collaborators.Select(u => u.UserId);
                return collaboratorIds.Contains(userId) && !hasRated;
            }

            return false;
        }

        /// <summary>
        /// Determines whether this instance has been rated by the user with the
        /// specified id
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>
        ///   <c>true</c> if this instance can can be rated by the user with 
        ///   the specified id; otherwise, <c>false</c>.
        /// </returns>
        public bool HasRated(int userId)
        {
            if (Ratings == null)
                return false;
            else
                return Ratings.Where(r => r.UserId == userId)
                    .FirstOrDefault() != null;
        }
    }

}