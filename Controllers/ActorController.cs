using BookingMovies.Data;
using BookingMovies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingMovies.Controllers
{
    public class ActorController : Controller
    { ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult Index(int actorid)
        {
            // to show its movies
            ViewBag.Actor = context.ActorMovies.Include(e => e.Actor)
                .Include(e => e.Movie).Where(e => e.ActorsId == actorid);

            var Actor = context.Actors.Where(e => e.Id == actorid).ToList();
            return View(Actor);
        }

      

    }
}
