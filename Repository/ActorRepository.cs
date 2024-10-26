using BookingMovies.Data;
using BookingMovies.Models;
using BookingMovies.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookingMovies.Repository
{
    public class ActorRepository : IActorRepository ,ISearchServices<Actor>
    {
        private readonly ApplicationDbContext context;

        public ActorRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IQueryable<ActorMovie> GetOneActorMovies(int actorid)
        {
           var result= context.ActorMovies.Include(e => e.Actor)
               .Include(e => e.Movie).Where(e => e.ActorsId == actorid);
            return result;
        }
        public IQueryable<ActorMovie> GetActorsMovies()
        {
            var result = context.ActorMovies.Include(e => e.Actor).Include(e => e.Movie);
            return result;
        }

        public List<Actor> Search(string SearchWord) { 
        
        var result= context.Actors.Where(e => e.FirstName.ToLower()
                .Contains(SearchWord.ToLower()) || e.LastName.ToLower().Contains(SearchWord.ToLower())
                || (e.FirstName.ToLower() + " " + e.LastName.ToLower()).Contains(SearchWord.ToLower())
                ).ToList();
            return result;
        }


    }
}
