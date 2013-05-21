using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BrewersBuddy.Models
{
    
    /// <summary>
    /// Represents the various types of batches that can be created
    /// </summary>
    public enum BatchType
    {
        Beer,
        Mead,
        Wine,
        Other,
    }


    /// <summary>
    /// Represents the various actions that can be performed on batches.
    /// </summary>
    public enum ActionType
    {
        Additives,
        Bottle,
        Start,
        Rack,
        Other,
    }


    public enum ContainerVolumeUnits
    {
        mL,
        L,
        gal,
    }

    public enum ContainerType
    {
        Bottle,
        Keg,
        Barrel,
    }




    [Table("Batch")]
    public class Batch
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BatchId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public BatchType Type { get; set;  }
        public UserProfile Owner { get; set; }

        public virtual ICollection<Measurement> Measurements { get; set; }
        public virtual ICollection<BatchNote> Notes { get; set; }
        public virtual ICollection<BatchAction> Actions { get; set; }
        public virtual ICollection<UserProfile> Collaborators { get; set; }
    }

    [Table("BatchNote")]
    public class BatchNote
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }
        public string Title { get; set; }
        public DateTime AuthorDate { get; set; }
        public string Text { get; set; }
        public Batch Batch { get; set; }
        public UserProfile Author { get; set; }

    }


    [Table("BatchAction")]
    public class BatchAction
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ActionId { get; set; }
        public string Title { get; set; }
        public DateTime ActionDate { get; set; }
        public string Description { get; set; }
        public ActionType Type { get; set; }
        public Batch Batch { get; set; }
        public UserProfile Performer { get; set; }
    }


    [Table("Measurement")]
    public class Measurement
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MeasurementId { get; set; }
        public string Name { get; set; }
        public DateTime MeasurementDate { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        //This is what was measured, such as SO2, ABV, gravity
        public String Measured { get; set; }
        public Batch Batch { get; set; }
    }

    [Table("Ingredient")]
    public class Ingredient
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }

        public virtual ICollection<Recipe> Recipes { get; set; }
    }


    [Table("Recipe")]
    public class Recipe
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }


    [Table("Container")]
    public class Container
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ContainerId { get; set; }
        public string Name { get; set; }
        public double Volume { get; set; }
        public ContainerVolumeUnits Unit { get; set; }
        public ContainerType Type { get; set; }
        public Batch Batch { get; set; }
    }


    [Table("Cellar")]
    public class Cellar
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CellarId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UserProfile Owner { get; set; }

        public virtual ICollection<Container> Containers { get; set; }
    }

    ////////////////////CONTEXTS/////////////////////////////////////
    public class BatchDBContext : DbContext
    {
        public DbSet<Batch> Batches { get; set; }
        public DbSet<BatchNote> BatchNotes { get; set; }
        public DbSet<BatchAction> BatchActions { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<Cellar> Cellars { get; set; }
    }
}