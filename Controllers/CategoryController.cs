using BookingMovies.Data;
using BookingMovies.Models;
using BookingMovies.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingMovies.Controllers
{
 
    public class CategoryController : Controller
    {
        private readonly IDataCrudRepository<Category> dbcategory;
        private readonly ICategoryRepository categoryRepository;
        public CategoryController(IDataCrudRepository<Category> Dbcategory,ICategoryRepository categoryRepository)
        {
            dbcategory = Dbcategory;
           this.categoryRepository = categoryRepository;
         
        }

       

        public IActionResult Index()
        {
            var categories=dbcategory.GetAll();
            return View(categories);
        }

        public IActionResult Movies(int categoryid)
        {
            var categories = categoryRepository.GetOneCategoryAll(categoryid);

            return View(categories);
        }
 

    }
}
