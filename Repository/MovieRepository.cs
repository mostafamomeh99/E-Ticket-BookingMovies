using BookingMovies.Data;
using BookingMovies.Models;
using BookingMovies.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookingMovies.Repository
{
    public class MovieRepository : IMovieRepository , ISearchServices<Movie>
    {
        private readonly ApplicationDbContext context;

        public MovieRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Movie> GetMoviesAll()
        {
            var result = context.Movies.Include(e => e.Category).Include(e => e.Cinema);
            return result;
        }

        public Movie GetMovieDetails(int movieid)
        {
            var result =context.Movies.Include(e => e.Category).Include(e => e.Cinema)
                .Include(e => e.ActorMovies)
                .ThenInclude(e => e.Actor)
                .ToList().FirstOrDefault(e => e.Id == movieid);
            return result;
        }

      public List<Movie> Search(string SearchWord)
        {
            var result = context.Movies.Where(e => e.Name.ToLower().Contains(SearchWord.ToLower()))
                .Include(e => e.Category).Include(e => e.Cinema)
                .ToList();

            return result;
        }

    }
}
