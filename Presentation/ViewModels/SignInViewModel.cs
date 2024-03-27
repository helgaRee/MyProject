using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels;

public class SignInViewModel
{
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Enter your email", Order = 1)]
    [Required(ErrorMessage = "A valid email is required")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Ogiltig e-postadress")]
    public string Email { get; set; } = null!;



    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter your Password", Order = 2)]
    [Required(ErrorMessage = "Password is required")]
    //[RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]{2,}$",
    //    ErrorMessage = "Lösenordet måste innehålla minst en stor bokstav, en liten bokstav, ett nummer och ett specialtecken.")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
}