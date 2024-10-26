using BookingMovies.Data;
using BookingMovies.Models;
using BookingMovies.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookingMovies.Repository
{
    public class CategoryRepository :ICategoryRepository
    {
        private readonly ApplicationDbContext context;

        public CategoryRepository(ApplicationDbContext context) {
            this.context = context;
        }

      public Category? GetOneCategoryAll(int id)
        {
            var result= context.Categories.Include(e=>e.Movies).ThenInclude(e => e.Cinema)
                .Where(e => e.Id == id)
                .FirstOrDefault();
            if(result == null)
            {
                return null;
            }
            return result;
        }




    }
}
