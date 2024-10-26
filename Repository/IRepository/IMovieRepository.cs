using BookingMovies.Models;

namespace BookingMovies.Repository.IRepository
{
    public interface IMovieRepository
    {
        IQueryable<Movie> GetMoviesAll();
        Movie GetMovieDetails(int movieid);
    }
}
