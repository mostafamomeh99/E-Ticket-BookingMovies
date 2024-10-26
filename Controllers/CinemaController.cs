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

        public CinemaController(IDataCrudRepository<Cinema> DatabaseCinema, ICinemaRepository CinemaRepository
            , IDataCrudRepository<Movie> DatabaseMovie
            )
        {
            this.DatabaseCinema = DatabaseCinema;
            this.CinemaRepository = CinemaRepository;
            this.DatabaseMovie = DatabaseMovie;
        }
        public IActionResult Index()
        {
            var cinema = DatabaseCinema.GetAll().ToList();
                //context.Cinemas.ToList();

            return View(cinema);
        }

        public IActionResult Movies(int cinemaid)
        {
            var cinema = CinemaRepository.GetCinemaAll(cinemaid).FirstOrDefault();

                //context.Cinemas.Where(e => e.Id == cinemaid).Include(e => e.Movies).ThenInclude(e => e.Category)
                //.FirstOrDefault();

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
                    var filename = Guid.NewGuid().ToString() + CinemaLogo.FileName;
                    var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinemas", filename);
                    using (var stream = System.IO.File.Create(pathname))
                    {
                        CinemaLogo.CopyTo(stream);
                    }
                    cinema.CinemaLogo = filename;

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

                var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinemas", oldcinema.CinemaLogo);
                if (CinemaLogo != null)
                {
                    var filename = Guid.NewGuid().ToString() + CinemaLogo.FileName;
                    var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinemas", filename);

                    if (System.IO.File.Exists(oldpath))
                    { System.IO.File.Delete(oldpath); }

                    using (var stream = System.IO.File.Create(pathname))
                    {
                        CinemaLogo.CopyTo(stream);
                    }
                    cinema.CinemaLogo = filename;

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
            var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cinemas", cinema.CinemaLogo);
            if (System.IO.File.Exists(oldpath))
            { System.IO.File.Delete(oldpath); }
            DatabaseCinema.Delete(cinema);
            DatabaseCinema.Commit();
            return RedirectToAction("Index", "Home");
        }

    }
}
