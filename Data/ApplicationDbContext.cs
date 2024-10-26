using Microsoft.EntityFrameworkCore;
using BookingMovies.Models;
using System.Reflection.Metadata;
namespace BookingMovies.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorMovie> ActorMovies { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
        public ApplicationDbContext()
   : base()
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();

        //    var connection = builder.GetConnectionString("DefaultConnection");
        //    optionsBuilder.UseSqlServer(connection);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActorMovie>()
                .HasKey(e => new { e.ActorsId, e.MoviesId });

            modelBuilder.Entity<ActorMovie>()
            .HasOne(e => e.Actor)
            .WithMany(e => e.ActorMovies)
            .HasForeignKey(e => e.ActorsId);

            modelBuilder.Entity<ActorMovie>()
                  .HasOne(e => e.Movie)
            .WithMany(e => e.ActorMovies)
            .HasForeignKey(e=>e.MoviesId);
            
        }

    }
}
