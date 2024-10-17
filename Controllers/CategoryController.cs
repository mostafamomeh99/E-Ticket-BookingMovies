using BookingMovies.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingMovies.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationDbContext context=new ApplicationDbContext();
        public IActionResult Index()
        {
            var categories=context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Movies(int categoryid)
        {
            var categories = context.Categories.Where(e => e.Id == categoryid).Include(e => e.Movies).
                ThenInclude(e=>e.Cinema)
                .FirstOrDefault();
            return View(categories);
        }

    }
}
