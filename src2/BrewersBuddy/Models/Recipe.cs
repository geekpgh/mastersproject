using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Security;

namespace BrewersBuddy.Models
{
    public class Recipe
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Add Date")]
        //Show just the date
        [DataType(DataType.Date)]
        public DateTime AddDate { get; set; }
        public int OwnerId { get; set; }
        public string Costs { get; set; }
        public string Prep { get; set; }
        public string Process { get; set; }
        public string Finishing { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }

        [ForeignKey("OwnerId")]
        public virtual UserProfile Owner { get; set; }


        public bool IsOwner(int userId)
        {
            return userId == OwnerId;
        }


        public bool CanEdit(int userId)
        {
            //Only the owner can edit
            return IsOwner(userId);
        }


        public bool CanView(int userId)
        {
            if(IsOwner(userId))
            {
                return true;
            }

            //Friends may view
            foreach(Friend friend in Owner.Friends)
            {
                if (friend.UserId == userId || friend.FriendUserId == userId)
                {
                    return true;
                }
            }

            return false;
        }
    }

}