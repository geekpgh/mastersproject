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
        public string Costs { get; set; }
        public string Prep { get; set; }
        public string Process { get; set; }
        public string Finishing { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }

}