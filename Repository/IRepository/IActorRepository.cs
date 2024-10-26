using BookingMovies.Models;

namespace BookingMovies.Repository.IRepository
{
    public interface IActorRepository
    {
        IQueryable<ActorMovie> GetOneActorMovies(int actorid);
        IQueryable<ActorMovie> GetActorsMovies();
    }
}
