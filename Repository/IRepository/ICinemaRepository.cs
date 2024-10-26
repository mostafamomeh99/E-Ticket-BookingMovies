using BookingMovies.Models;

namespace BookingMovies.Repository.IRepository
{
    public interface ICinemaRepository
    {
        IQueryable<Cinema> GetCinemaAll(int cinemaid);
    }
}
