namespace BookingMovies.Repository.IRepository
{
    public interface IFileServices<T>
    {
        string AddFile(IFormFile Picture, string file);
        void DeleteFile( string file , T entity);
    }
}
