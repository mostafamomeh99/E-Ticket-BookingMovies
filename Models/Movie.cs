using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BookingMovies.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Name is too short")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Display(Name = "Movie Price")]
        [Required]
        [Range(10,500,ErrorMessage ="price is between 10$ and 500$")]
        public double Price { get; set; }
        [Display(Name = "Movie Image")]
        public string ImgUrl { get; set; }
        [Display(Name = "Movie Trailer Link")]
        [Required]
        [RegularExpression(@"^https:\/\/(www\.youtube\.com\/watch\?v=([a-zA-Z0-9_-]{11})(&.*)?|youtu\.be\/([a-zA-Z0-9_-]{11})(\?si=[a-zA-Z0-9_-]*)?)$")]
        public string TrailerUrl { get; set; }
        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [Required]
        public DateTime EndDate { get; set; }
        [ValidateNever]
        public MovieStatus MovieStatus { get; set; }
        [Required]
        [Display(Name = "Available in Cinema")]
        public int CinemaId { get; set; }
        [Required]
        [Display(Name = "Movie Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Cinema Cinema {  get; set; }
        [ValidateNever]
        public Category Category { get; set; }

        public List<Actor> Actors { get; set; }=new List<Actor>(){ };
        public ICollection<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>() { };
    }
}
