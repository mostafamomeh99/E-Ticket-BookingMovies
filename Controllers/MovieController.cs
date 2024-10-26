using BookingMovies.Models;
using BookingMovies.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace BookingMovies.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository movieRepository;
        private readonly IDataCrudRepository<Movie> DatabaseMovie;
        private readonly IDataCrudRepository<Cinema> DatabaseCinema;
        private readonly IDataCrudRepository<Category> DatabaseCategory;
        public MovieController(IMovieRepository movieRepository,
            IDataCrudRepository<Movie> DatabaseMovie, IDataCrudRepository<Cinema> DatabaseCinema
            , IDataCrudRepository<Category> DatabaseCategory
            ) {
            this.movieRepository = movieRepository;
            this.DatabaseMovie = DatabaseMovie;
            this.DatabaseCinema = DatabaseCinema;
            this.DatabaseCategory = DatabaseCategory;
        }


        public IActionResult AddMovie()
        {
            

            ViewBag.CategoryId = DatabaseCategory.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(), 
                Text = c.Name  
            }).ToList();

            ViewBag.CinemaId= DatabaseCinema.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            return View(new Movie());
        }
        [HttpPost]
        public IActionResult AddMovie(Movie movie , IFormFile ImgUrl)
        {
            ModelState.Remove("ImgUrl");
            if(ModelState.IsValid)
            {
                if (ImgUrl !=null)
                {
                    var filename = Guid.NewGuid().ToString() + ImgUrl.FileName;
                    var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", filename);
                    using (var stream = System.IO.File.Create(pathname))
                    {
                        ImgUrl.CopyTo(stream);
                    }
                    movie.ImgUrl = filename;
                    if(DateTime.Now>movie.EndDate && DateTime.Now > movie.StartDate)
                    {movie.MovieStatus=BookingMovies.Models.MovieStatus.Expired; }

                    else if (DateTime.Now < movie.EndDate && DateTime.Now > movie.StartDate)
                    { movie.MovieStatus = BookingMovies.Models.MovieStatus.Available; }

                    else { movie.MovieStatus = BookingMovies.Models.MovieStatus.Upcoming; }

                    DatabaseMovie.Create(movie);
                    DatabaseMovie.Commit();
                    return RedirectToAction("Index", "Home");

                }
                else{
                    ModelState.AddModelError(string.Empty, "upload photo");
                    ViewBag.CategoryId = DatabaseCategory.GetAll().Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();

                    ViewBag.CinemaId = DatabaseCinema.GetAll().Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList();

                    return View(movie);
                }
            
            }
            else
            {
                ViewBag.CategoryId = DatabaseCategory.GetAll().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                ViewBag.CinemaId = DatabaseCinema.GetAll().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                return View(movie);
            }

        }

        public IActionResult UpdateMovie(int id)
        {
            var movie = DatabaseMovie.GetById(id);

            ViewBag.CategoryId = DatabaseCategory.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = c.Id == movie.CinemaId
            }).ToList();

            ViewBag.CinemaId = DatabaseCinema.GetAll().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name ,
                Selected = c.Id == movie.CinemaId
            }).ToList();

            return View(movie);
        }

        [HttpPost]
        public IActionResult UpdateMovie(Movie movie, IFormFile ImgUrl)
        {

            var oldmovie= DatabaseMovie.GetAll().AsNoTracking().FirstOrDefault(e=>e.Id==movie.Id);

            ModelState.Remove("ImgUrl");
            if (ModelState.IsValid)
            {
                if (ImgUrl != null)
                {
                    var filename = Guid.NewGuid().ToString() + ImgUrl.FileName;
                    var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", filename);
                    var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", oldmovie.ImgUrl);
                    if (System.IO.File.Exists(oldpath))
                    { System.IO.File.Delete(oldpath); }
                    using (var stream = System.IO.File.Create(pathname))
                    {
                        ImgUrl.CopyTo(stream);
                    }
                    
                    movie.ImgUrl = filename;

                    if (DateTime.Now > movie.EndDate && DateTime.Now > movie.StartDate)
                    { movie.MovieStatus = BookingMovies.Models.MovieStatus.Expired; }

                    else if (DateTime.Now < movie.EndDate && DateTime.Now > movie.StartDate)
                    { movie.MovieStatus = BookingMovies.Models.MovieStatus.Available; }

                    else { movie.MovieStatus = BookingMovies.Models.MovieStatus.Upcoming; }

                    DatabaseMovie.Update(movie);
                    DatabaseMovie.Commit();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    movie.ImgUrl = oldmovie.ImgUrl;

                    if (DateTime.Now > movie.EndDate && DateTime.Now > movie.StartDate)
                    { movie.MovieStatus = BookingMovies.Models.MovieStatus.Expired; }

                    else if (DateTime.Now < movie.EndDate && DateTime.Now > movie.StartDate)
                    { movie.MovieStatus = BookingMovies.Models.MovieStatus.Available; }

                    else { movie.MovieStatus = BookingMovies.Models.MovieStatus.Upcoming; }
                    DatabaseMovie.Update(movie);
                    DatabaseMovie.Commit();
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.CategoryId = DatabaseCategory.GetAll().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == movie.CinemaId
                }).ToList();

                ViewBag.CinemaId = DatabaseCinema.GetAll().Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = c.Id == movie.CinemaId
                }).ToList();

                return View(movie);
            }
        }
        public IActionResult DeleteMovie(int id)
        {
            var movie = DatabaseMovie.GetById(id);
            var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", movie.ImgUrl);
            if (System.IO.File.Exists(oldpath))
            { System.IO.File.Delete(oldpath); }
            return RedirectToAction("Index", "Home");
        }

    }
}
