using Microsoft.AspNetCore.Identity;

namespace BookingMovies.Models
{
    public class ApplicationUser : IdentityUser
    {
       public string? Address { get; set; }
    }
}
