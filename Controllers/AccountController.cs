using BookingMovies.Models;
using BookingMovies.Models.VIEWModel;
using BookingMovies.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookingMovies.Controllers
{
    //[ValidateAntiForgeryToken]
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IAccountRepository accountRepository;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager , IAccountRepository accountRepository 
            , RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.accountRepository = accountRepository;
            this.roleManager = roleManager;
        }
    

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  Register(RegisterVm newaccout)
        {


            if (ModelState.IsValid)
            {
                var checkemail = accountRepository.CheckUniqueEmail(newaccout.Email);
                if (checkemail) 
                { 
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    UserName = accountRepository.CreateUserName(newaccout.FirstName, newaccout.LastName),
                    Email = newaccout.Email,
                    Address = newaccout.Address
                };
                
                var result = await userManager.CreateAsync(applicationUser, newaccout.Password);
                if (result.Succeeded)
                {
                        await userManager.AddToRoleAsync(applicationUser, "user");
                        return RedirectToAction( "Login", "Account"); }
              
                else
                {
                    foreach (var error in result.Errors)
                    { ModelState.AddModelError(string.Empty, error.Description); }
                    return View();
                }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "there is already Account with this email");
                    return View();
                }

            }
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm login)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(login.Email);
            if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Email");
                    return View();
                }
           
                else
                {
                    var checkpass=await userManager.CheckPasswordAsync(user, login.Password);

                    if(checkpass)
                    {

                        await signInManager.SignInAsync(user, true);
                        var roles = await userManager.GetRolesAsync(user);
                        Console.WriteLine($"User Roles: {string.Join(", ", roles)}");

                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Password");
                        return View();
                    }


                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");
                return View();
            }
           
        }

        public async Task <IActionResult> Logout()
        {

            await signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}
