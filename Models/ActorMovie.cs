namespace BookingMovies.Models
{
    public class ActorMovie
    {
        public int ActorsId { get; set; }
        public int MoviesId { get; set; }
        public Actor Actor { get; set; }
        public Movie Movie { get; set; }


    }
}
