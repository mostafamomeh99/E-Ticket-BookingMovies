using BookingMovies.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingMovies.Controllers
{
    public class CinemaController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult Index()
        {
            var cinema = 
                context.Cinemas.ToList();
     
            return View(cinema);
        }

        public IActionResult Movies(int cinemaid)
        {
            var cinema =
                context.Cinemas.Where(e => e.Id == cinemaid).Include(e => e.Movies).ThenInclude(e => e.Category)
                .FirstOrDefault();

            return View(cinema);
        }

    }
}
