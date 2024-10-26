using BookingMovies.Data;
using BookingMovies.Models;
using BookingMovies.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookingMovies.Repository
{
    public class CinemaRepository : ICinemaRepository
    {
        private readonly ApplicationDbContext context;

        public CinemaRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Cinema> GetCinemaAll(int cinemaid) {
            var result = context.Cinemas.Where(e => e.Id == cinemaid)
                .Include(e => e.Movies).ThenInclude(e => e.Category);

               return result;
        }

    }
}
