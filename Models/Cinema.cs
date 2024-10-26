using System.ComponentModel.DataAnnotations;

namespace BookingMovies.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Name is too short")]
        [MaxLength(15, ErrorMessage = "Name is too Long")]
        public string Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Name is too short")]
        public string Description { get; set; }
        public string CinemaLogo { get; set; }
        [Required]
        public string Address { get; set; }
        public ICollection<Movie> Movies { get; set; }=new List<Movie>() { };
    }
}
