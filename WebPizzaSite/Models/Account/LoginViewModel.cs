using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebPizzaSite.Models.Account;

public class LoginViewModel
{
    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; } = string.Empty;
    
    [Display(Name = "Password")]
    [Required(ErrorMessage = "Please enter a password.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}