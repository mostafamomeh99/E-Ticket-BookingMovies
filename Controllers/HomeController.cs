using BookingMovies.Data;
using BookingMovies.Models;
using BookingMovies.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace BookingMovies.Controllers
{

    public class HomeController : Controller
    {
        private readonly IDataCrudRepository<Movie> DatabaseMovie;
        private readonly IMovieRepository movieRepository;
        private readonly IActorRepository actorRepository;
        private readonly ILogger<HomeController> _logger;

        private readonly ISearchServices<Movie> searchServicesMovie;
        private readonly ISearchServices<Actor> searchServicesActor;

        public HomeController(ILogger<HomeController> logger,IMovieRepository movieRepository,
            IDataCrudRepository<Movie> DatabaseMovie , ISearchServices<Movie> searchServicesMovie ,
            ISearchServices<Actor> searchServicesActor , IActorRepository actorRepository
            )
        {
            this.DatabaseMovie= DatabaseMovie;
            this.searchServicesMovie = searchServicesMovie;
            this.searchServicesActor = searchServicesActor;

            this.actorRepository = actorRepository;
            this.movieRepository= movieRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var movies = movieRepository.GetMoviesAll().ToList();
            return View(movies);
        }

        public IActionResult Details(int movieid)
        {
            var movie = movieRepository.GetMovieDetails(movieid);

            return View(movie);
        }

        [HttpPost]
        public IActionResult Search(string SearchWord,string SearchType)
        {
            if(SearchType== "Movies")
            {
                var movies = searchServicesMovie.Search(SearchWord);
                if (movies.Count== 0)
                { return RedirectToAction("NotFoundSearch"); }

                return View(movies);
            }
            else if (SearchType == "Actors")
            {
                List<Actor> Actor = searchServicesActor.Search(SearchWord);

                if(Actor.Count==0)
                { return RedirectToAction("NotFoundSearch"); }
                TempData["actors"] = Actor;
                ViewBag.actormoviesearch = actorRepository.GetActorsMovies().ToList();
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
