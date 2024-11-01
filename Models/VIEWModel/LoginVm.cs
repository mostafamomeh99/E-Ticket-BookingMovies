using System.ComponentModel.DataAnnotations;

namespace BookingMovies.Models.VIEWModel
{
    public class LoginVm
    {
        public int Id { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$", ErrorMessage = "Invalid E-mail")]
        [Required(ErrorMessage = "Email is Required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PassWord is Required.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+=[\]{};':""\\|,.<>/?-]).{8,}$",
           ErrorMessage = "Password must contain at least one capital , small character , special character with minumum length 8")]
        [DataType("password")]
        public string Password { get; set; }
    }
}
