using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BookingMovies.Models
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [MinLength(3, ErrorMessage = "Name is too short")]
        [MaxLength(15 ,ErrorMessage ="Name is too Long")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [MinLength(3, ErrorMessage = "Name is too short")]
        [MaxLength(15, ErrorMessage = "Name is too Long")]
        public string LastName { get; set; }
        [Required]
        public string Bio { get; set; }
        [Required]
        [Display(Name = "Profile Picture")]
        public  string ProfilePicture { get; set; }
        [Required]
        [Display(Name = "News Paragraph")]
        public string News { get; set; }
        [ValidateNever]
        public  List<Movie> Movies { get; set; } = new List<Movie>() { };
        [ValidateNever]
        public  ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>() { };
    }
}
