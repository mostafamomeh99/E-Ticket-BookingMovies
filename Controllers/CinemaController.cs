using BookingMovies.Data;
using BookingMovies.Models;
using BookingMovies.Repository;
using BookingMovies.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingMovies.Controllers
{
    public class CinemaController : Controller
    {
        private readonly IDataCrudRepository<Cinema> DatabaseCinema;
        private readonly ICinemaRepository CinemaRepository;
        private readonly IDataCrudRepository<Movie> DatabaseMovie;
        private readonly IFileServices<Cinema> Cinemafiles;
 
        public CinemaController(IDataCrudRepository<Cinema> DatabaseCinema, ICinemaRepository CinemaRepository
            , IDataCrudRepository<Movie> DatabaseMovie ,
            IFileServices<Cinema> Cinemafiles
            )
        {
            this.DatabaseCinema = DatabaseCinema;
            this.CinemaRepository = CinemaRepository;
            this.DatabaseMovie = DatabaseMovie;
            this.Cinemafiles = Cinemafiles;
        }

        public IActionResult Index()
        {
            var cinema = DatabaseCinema.GetAll().ToList();
            return View(cinema);
        }


        public IActionResult Movies(int cinemaid)
        {
            var cinema = CinemaRepository.GetCinemaAll(cinemaid).FirstOrDefault();
            return View(cinema);
        }


        public IActionResult AddCinema()
        {
            return View(new Cinema());
        }
        [HttpPost]
        public IActionResult AddCinema(Cinema cinema, IFormFile CinemaLogo)
        {
            ModelState.Remove("CinemaLogo");
            if (ModelState.IsValid)
            {
                if (CinemaLogo != null)
                {
                    cinema.CinemaLogo = Cinemafiles.AddFile(CinemaLogo, "cinemas");
                    DatabaseCinema.Create(cinema);
                    DatabaseCinema.Commit();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "please Upload Photo");
                    return View(cinema);
                }
               
            }
            return View(new Cinema());
        }




        public IActionResult UpdateCinema(int id)
        {
            var cinema = DatabaseCinema.GetById(id);
            return View(cinema);
        }
        [HttpPost]
        public IActionResult UpdateCinema(Cinema cinema , IFormFile CinemaLogo)
        {
            ModelState.Remove("CinemaLogo");
            var oldcinema = DatabaseCinema.GetAll().AsNoTracking().FirstOrDefault(e => e.Id == cinema.Id);
            if (ModelState.IsValid)
            {
                if (CinemaLogo != null)
                {
                    cinema.CinemaLogo = Cinemafiles.AddFile(CinemaLogo, "cinemas");
                    Cinemafiles.DeleteFile("cinemas", oldcinema);
                    DatabaseCinema.Update(cinema);
                    DatabaseCinema.Commit();
                 
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    cinema.CinemaLogo = oldcinema.CinemaLogo;
                    DatabaseCinema.Update(cinema);
                    DatabaseCinema.Commit();
                    return RedirectToAction("Index", "Home");
                }

            }

            return View(cinema);
        }

        public IActionResult DeleteCinema(int id)
        {
            var cinema = DatabaseCinema.GetById(id);
            Cinemafiles.DeleteFile("cinemas", cinema);
            DatabaseCinema.Delete(cinema);
            DatabaseCinema.Commit();
            return RedirectToAction("Index", "Home");
        }

    }
}
