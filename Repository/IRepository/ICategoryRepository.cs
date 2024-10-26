using BookingMovies.Models;

namespace BookingMovies.Repository.IRepository
{
    public interface ICategoryRepository
    {
        Category? GetOneCategoryAll(int id);
    }
}
