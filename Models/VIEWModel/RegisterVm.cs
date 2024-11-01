using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.ComponentModel.DataAnnotations;

namespace BookingMovies.Models.VIEWModel
{
    public class RegisterVm 
    {
        public int Id { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_.]{1,50}$", ErrorMessage = "Invalid name")]
        [Display(Name="First Name")]
        [Required( ErrorMessage = "First Name is Required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required.")]
        [RegularExpression(@"^[a-zA-Z0-9_.]{1,50}$", ErrorMessage = "Invalid name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$", ErrorMessage = "Invalid E-mail")]
        [Required(ErrorMessage = "Email is Required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Address is Required.")]
        [RegularExpression(@"^[a-zA-Z0-9_.]{1,70}$", ErrorMessage = "Invalid Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "PassWord is Required.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+=[\]{};':""\\|,.<>/?-]).{8,}$",
            ErrorMessage = "Password must contain at least one capital , small character , special character with minumum length 8")]
        [DataType("password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please, Confirm PassWord")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType("password")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+=[\]{};':""\\|,.<>/?-]).{8,}$",
            ErrorMessage = "Password must contain at least one capital , small character , special character with minumum length 8")]
        public string ConfirmPassword { get; set; }

    }
}
