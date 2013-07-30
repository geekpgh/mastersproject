using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BrewersBuddy.Models
{
	public class BrewersBuddyContext : DbContext
	{
		public BrewersBuddyContext()
			: base("DefaultConnection")
		{
		}

		public DbSet<Batch> Batches { get; set; }
		public DbSet<BatchNote> BatchNotes { get; set; }
		public DbSet<BatchAction> BatchActions { get; set; }
		public DbSet<Measurement> Measurements { get; set; }
		public DbSet<Recipe> Recipes { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<Container> Containers { get; set; }
		public DbSet<BatchRating> BatchRatings { get; set; }
		public DbSet<BatchComment> BatchComments { get; set; }

		public DbSet<Friend> Friends { get; set; }
		public DbSet<UserProfile> UserProfiles { get; set; }
		public DbSet<webpages_Membership> webpages_Memberships { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
            
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

			modelBuilder.Entity<Ingredient>()
				.HasMany(c => c.Recipes)
				.WithMany(i => i.Ingredients)
				.Map(t => t.MapLeftKey("IngredientID")
					.MapRightKey("RecipeID")
					.ToTable("IngredientRecipe"));

            modelBuilder.Entity<BatchRating>()
                .HasRequired(t => t.User)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BatchComment>()
                .HasRequired(t => t.User)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BatchNote>()
                .HasRequired(t => t.Author)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BatchAction>()
                .HasRequired(t => t.Performer)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Friend>()
                .HasRequired(t => t.FriendUser)
                .WithMany()
                .WillCascadeOnDelete(false);

            // Set up the many-to-many relationship between batches
            // and collaborators
            modelBuilder.Entity<Batch>()
                .HasMany(b => b.Collaborators)
                .WithMany(c => c.CollaboratorBatches)
                .Map(t => t.MapLeftKey("BatchId")
                    .MapRightKey("UserId")
                    .ToTable("BatchCollaborator"));
		}

	}
}