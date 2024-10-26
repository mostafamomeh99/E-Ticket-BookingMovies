namespace BookingMovies.Repository.IRepository
{
    public interface ISearchServices<T> where T : class
    {
        List<T> Search(string SearchWord);
    }
}
