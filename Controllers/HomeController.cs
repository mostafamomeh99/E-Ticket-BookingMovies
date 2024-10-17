using BookingMovies.Data;
using BookingMovies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace BookingMovies.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var movies = context.Movies.Include(e => e.Category).Include(e => e.Cinema).ToList();

            return View(movies);
        }

        public IActionResult Details(int movieid)
        {
            var movie = context.Movies.Include(e => e.Category).Include(e => e.Cinema)
                .Include(e=>e.ActorMovies)
                .ThenInclude(e=>e.Actor)
                .ToList().FirstOrDefault(e=>e.Id== movieid);

            return View(movie);
        }

        [HttpPost]
        public IActionResult Search(string SearchWord,string SearchType)
        {
            if(SearchType== "Movies")
            {
                var movies = context.Movies.Where(e => e.Name.ToLower().Contains(SearchWord.ToLower()))
                .Include(e => e.Category).Include(e => e.Cinema)
                .ToList();
                if (movies.Count == 0)
                { return RedirectToAction("NotFoundSearch"); }

                return View(movies);
            }
            else if (SearchType == "Actors")
            {
                List<Actor> Actor = context.Actors.Where(e =>e.FirstName.ToLower()
                .Contains(SearchWord.ToLower()) || e.LastName.ToLower().Contains(SearchWord.ToLower())
                || (e.FirstName.ToLower()+" "+ e.LastName.ToLower()).Contains(SearchWord.ToLower())
                ).ToList();

                if(Actor.Count==0)
                { return RedirectToAction("NotFoundSearch"); }
                TempData["actors"] = Actor;
                ViewBag.actormoviesearch = context.ActorMovies.Include(e => e.Actor).Include(e => e.Movie).ToList();
                return View(Actor);
            }
            return RedirectToAction("NotFoundSearch");
        }

    


        public IActionResult NotFoundSearch()
        {

            return View();
        }










        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
