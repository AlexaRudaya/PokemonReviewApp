using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Data
{
    public class DataContext : DbContext 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<Pokemon> Pokemon { get; set; }

        public DbSet<PokemonOwner> PokemonOwners { get; set; }

        public DbSet<PokemonCategory> PokemonCategories { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonCategory>()
                .HasKey(_ => new { _.PokemonId, _.CategoryId });

            modelBuilder.Entity<PokemonCategory>()
                .HasOne(_ => _.Pokemon)
                .WithMany(_ => _.PokemonCategories)
                .HasForeignKey(_ => _.PokemonId);

            modelBuilder.Entity<PokemonCategory>()
                .HasOne(_ => _.Category)
                .WithMany(_ => _.PokemonCategories)
                .HasForeignKey(_ => _.CategoryId);

            modelBuilder.Entity<PokemonOwner>()
                .HasKey(_ => new { _.PokemonId, _.OwnerId });

            modelBuilder.Entity<PokemonOwner>()
                .HasOne(_ => _.Pokemon)
                .WithMany(_ => _.PokemonOwners)
                .HasForeignKey(_ => _.PokemonId);

            modelBuilder.Entity<PokemonOwner>()
                .HasOne(_ => _.Owner)
                .WithMany(_ => _.PokemonOwners)
                .HasForeignKey(_ => _.OwnerId);
        }
    }
}
