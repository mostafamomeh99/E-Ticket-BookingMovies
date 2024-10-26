using BookingMovies.Data;
using BookingMovies.Models;
using BookingMovies.Repository;
using BookingMovies.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookingMovies
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(
                   options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                   );

            builder.Services.AddScoped<IDataCrudRepository<Category>, DataCrudRepository<Category>>();
            builder.Services.AddScoped<IDataCrudRepository<Cinema>, DataCrudRepository<Cinema>>();
            builder.Services.AddScoped<IDataCrudRepository<Actor>, DataCrudRepository<Actor>>();
            builder.Services.AddScoped<IDataCrudRepository<Movie>, DataCrudRepository<Movie>>();
            builder.Services.AddScoped<IDataCrudRepository<ActorMovie>, DataCrudRepository<ActorMovie>>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICinemaRepository, CinemaRepository>();
            builder.Services.AddScoped<IActorRepository, ActorRepository>();
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<ISearchServices<Movie>, MovieRepository>();
            builder.Services.AddScoped<ISearchServices<Actor>, ActorRepository>();
            builder.Services.AddScoped<IFileServices<Actor>, ActorRepository>();
            builder.Services.AddScoped<IFileServices<Cinema>, CinemaRepository>();
            builder.Services.AddScoped<IFileServices<Movie>, MovieRepository>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
