using BookingMovies.Data;
using BookingMovies.Models;
using BookingMovies.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BookingMovies.Repository
{
    public class CinemaRepository : ICinemaRepository , IFileServices<Cinema>
    {
        private readonly ApplicationDbContext context;

        public CinemaRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Cinema> GetCinemaAll(int cinemaid) {
            var result = context.Cinemas.Where(e => e.Id == cinemaid)
                .Include(e => e.Movies).ThenInclude(e => e.Category);

               return result;
        }



        public string AddFile(IFormFile Picture, string file)
        {
            var filename = Guid.NewGuid().ToString() + Picture.FileName;
            var pathname = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{file}", filename);
            using (var stream = System.IO.File.Create(pathname))
            {
                Picture.CopyTo(stream);
            }
            return filename;
        }
        public void DeleteFile(string file, Cinema cinema)
        {

            var oldpath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{file}", cinema.CinemaLogo);

            if (System.IO.File.Exists(oldpath))
            { System.IO.File.Delete(oldpath); }
        }



    }
}
