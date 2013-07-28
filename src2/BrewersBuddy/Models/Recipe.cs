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
        public DateTime AddDate { get; set; }
        public int OwnerId { get; set; }
        public string Cost { get; set; }
        public string Preparation { get; set; }
        public string Ingredient { get; set; }
        public string  Ferment { get; set; }
        public string Finish { get; set; }

		public virtual ICollection<Ingredient> Ingredients { get; set; }
    }
   
}