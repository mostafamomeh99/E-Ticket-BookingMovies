using BookingMovies.Data;
using BookingMovies.Models;
using BookingMovies.Repository;
using BookingMovies.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BookingMovies.Controllers
{
    public class ActorController : Controller
    {
        private readonly IDataCrudRepository<Actor> DatabaseActor;
        private readonly IActorRepository ActorRepository;
        private readonly IDataCrudRepository<Movie> DatabaseMovie;
        private readonly IDataCrudRepository<ActorMovie> DatabaseActorMovie;
        public ActorController(IDataCrudRepository<Actor> DatabaseActor, IActorRepository ActorRepository
            , IDataCrudRepository<Movie> DatabaseMovie , IDataCrudRepository<ActorMovie> DatabaseActorMovie
            )
        {
            this.DatabaseActor = DatabaseActor;
            this.ActorRepository = ActorRepository;
            this.DatabaseMovie = DatabaseMovie;
            this.DatabaseActorMovie = DatabaseActorMovie;
        }

        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult Index(int actorid)
        {
            // to show its movies
            ViewBag.Actor =ActorRepository.GetOneActorMovies(actorid);
            //context.ActorMovies.Include(e => e.Actor)
            //    .Include(e => e.Movie).Where(e => e.ActorsId == actorid);
            
            // to send actor with its details
            List<Actor> actor=new List<Actor>();
            actor.Add(DatabaseActor.GetById(actorid));

            return View (actor);
        }


        public IActionResult AddActor()
        {
            ViewBag.movies = DatabaseMovie.GetAll();
            return View(new Actor());
        }
        [HttpPost]
        public IActionResult AddActor(Actor actor ,List<int> MoviesSelect ,IFormFile ProfilePicture)
        {
            ModelState.Remove("ProfilePicture");
            if(ModelState.IsValid)
            { if(ProfilePicture != null)
                {  var filename=Guid.NewGuid().ToString()+ProfilePicture.FileName;
                    var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", filename);
                    using(var stream=System.IO.File.Create(pathname))
                    {
                        ProfilePicture.CopyTo(stream);
                    }
                    actor.ProfilePicture = filename;

                    DatabaseActor.Create(actor);
                    DatabaseActor.Commit();
                    foreach (int i in MoviesSelect)
                    { DatabaseActorMovie.Create(new ActorMovie() { ActorsId= actor.Id,MoviesId=i }); }
                    DatabaseActor.Commit();
                    return RedirectToAction("Index", "Home");

                }
                ModelState.AddModelError(string.Empty, "please Upload Photo");
                ViewBag.movies = DatabaseMovie.GetAll();
                return View(actor);
            }
            ViewBag.movies = DatabaseMovie.GetAll();
            return View(actor);
        }

        public IActionResult UpdateActor(int id)
        {
            var actor = DatabaseActor.GetById(id);
            ViewBag.movies = DatabaseMovie.GetAll();
            ViewBag.actormovies = ActorRepository.GetOneActorMovies(id).ToList() ;
            return View(actor);
        }
        [HttpPost]
        public IActionResult UpdateActor(Actor actor, List<int> MoviesSelect, IFormFile ProfilePicture)
        {
            ModelState.Remove("ProfilePicture");
            if (ModelState.IsValid)
            { 
                var oldactor = DatabaseActor.GetAll().AsNoTracking().FirstOrDefault(e=>e.Id==actor.Id);

                var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", oldactor.ProfilePicture);
                if (ProfilePicture != null)
                {   
                    var filename = Guid.NewGuid().ToString() + ProfilePicture.FileName;
                    var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", filename);
                  
                    if (System.IO.File.Exists(oldpath))
                    { System.IO.File.Delete(oldpath); }
                    
                    using (var stream = System.IO.File.Create(pathname))
                    {
                        ProfilePicture.CopyTo(stream);
                    }
                    actor.ProfilePicture = filename;

                    DatabaseActor.Update(actor);
                    DatabaseActor.Commit();
                    var oldmovies = ActorRepository.GetOneActorMovies(actor.Id).AsNoTracking().ToList();
                    foreach (int i in MoviesSelect)
                    {
                        if(oldmovies.Any(e=>e.MoviesId==i))
                        { continue; }
                        else
                        {
       DatabaseActorMovie.Create(new ActorMovie() { ActorsId = actor.Id, MoviesId = i });
                        }
              
                    }
                    DatabaseActor.Commit();
                    return RedirectToAction("Index", "Home");
                }
                else 
                {
                    actor.ProfilePicture = oldactor.ProfilePicture;
                    DatabaseActor.Update(actor);
                    DatabaseActor.Commit();
                    var oldmovies = ActorRepository.GetOneActorMovies(actor.Id).AsNoTracking().ToList();
                    foreach (int i in MoviesSelect)
                    {
                        if (oldmovies.Any(e => e.MoviesId == i))
                        { continue; }
                        else
                        {
                            DatabaseActorMovie.Create(new ActorMovie() { ActorsId = actor.Id, MoviesId = i });
                        }

                    }
                    DatabaseActor.Commit();
                    return RedirectToAction("Index", "Home");
                }
               
            }
            ViewBag.movies = DatabaseMovie.GetAll();
            ViewBag.actormovies = ActorRepository.GetOneActorMovies(actor.Id).ToList();
            return View(actor);
        }



        public IActionResult DeleteActor(int id)
        {
            var actor = DatabaseActor.GetById(id);
            var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\cast", actor.ProfilePicture);
            if (System.IO.File.Exists(oldpath))
            { System.IO.File.Delete(oldpath); }
            DatabaseActor.Delete(actor);
            DatabaseActor.Commit();
            return RedirectToAction("Index", "Home");
        }


    }
}
