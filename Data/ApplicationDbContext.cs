using Microsoft.EntityFrameworkCore;
using BookingMovies.Models;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BookingMovies.Models.VIEWModel;
namespace BookingMovies.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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
        public DbSet<BookingMovies.Models.VIEWModel.RegisterVm> RegisterVm { get; set; } = default!;
        public DbSet<BookingMovies.Models.VIEWModel.LoginVm> LoginVm { get; set; } = default!;

    }
}
