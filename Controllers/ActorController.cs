using BookingMovies.Data;
using BookingMovies.Models;
using BookingMovies.Repository;
using BookingMovies.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IFileServices<Actor> ActorFiles;

        public ActorController(IDataCrudRepository<Actor> DatabaseActor, IActorRepository ActorRepository
            , IDataCrudRepository<Movie> DatabaseMovie , IDataCrudRepository<ActorMovie> DatabaseActorMovie,
            IFileServices<Actor> ActorFiles
            )
        {
            this.DatabaseActor = DatabaseActor;
            this.ActorRepository = ActorRepository;
            this.DatabaseMovie = DatabaseMovie;
            this.DatabaseActorMovie = DatabaseActorMovie;
           this.ActorFiles = ActorFiles;
        }

        public IActionResult Index(int actorid)
        {
            // to show its movies
            ViewBag.Actor =ActorRepository.GetOneActorMovies(actorid);         
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
   
        [ValidateAntiForgeryToken]
        public IActionResult AddActor(Actor actor ,List<int> MoviesSelect ,IFormFile ProfilePicture)
        {
            ModelState.Remove("ProfilePicture");
            if(ModelState.IsValid)
            { if(ProfilePicture != null)
                {
                 
                    actor.ProfilePicture = ActorFiles.AddFile(ProfilePicture , "cast");
                 
                    DatabaseActor.Create(actor);
                    DatabaseActor.Commit();
                    foreach (int i in MoviesSelect)
                    { DatabaseActorMovie.Create(new ActorMovie() { ActorsId= actor.Id,MoviesId=i }); }
                    DatabaseActor.Commit();
                    return RedirectToAction("Index", "Actor", new { actorid= actor.Id });

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
      
        [ValidateAntiForgeryToken]
        public IActionResult UpdateActor(Actor actor, List<int> MoviesSelect, IFormFile ProfilePicture)
        {
            ModelState.Remove("ProfilePicture");
            if (ModelState.IsValid)
            { 
                var oldactor = DatabaseActor.GetAll().AsNoTracking().FirstOrDefault(e=>e.Id==actor.Id);

                if (ProfilePicture != null)
                {
                   
                    actor.ProfilePicture = ActorFiles.AddFile(ProfilePicture, "cast");
                    ActorFiles.DeleteFile( "cast", oldactor);
               
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
                    return RedirectToAction("Index", "Actor", new { actorid = actor.Id });
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
                    return RedirectToAction("Index", "Actor", new { actorid = actor.Id });
                }
               
            }
            ViewBag.movies = DatabaseMovie.GetAll();
            ViewBag.actormovies = ActorRepository.GetOneActorMovies(actor.Id).ToList();
            return View(actor);
        }




        public IActionResult DeleteActor(int id)
        {
            var actor = DatabaseActor.GetById(id);
            ActorFiles.DeleteFile("cast", actor);
            DatabaseActor.Delete(actor);
            DatabaseActor.Commit();
            return RedirectToAction("Index", "Home");
        }


    }
}
