using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels.Account;

public class BasicInfoFormViewModel
{

    public string UserId { get; set; } = null!;

    [Required(ErrorMessage = "Du måste ange ett giltigt förnamn")]
    [DataType(DataType.Text)]
    [Display(Name = "First name", Prompt = "Skriv ditt förnamn")]
    public string FirstName { get; set; } = null!;




    [Required(ErrorMessage = "Du måste fylla i ett giltigt efternamn")]
    [DataType(DataType.Text)]
    [Display(Name = "Last name", Prompt = "skriv in ditt efternamn")]
    public string LastName { get; set; } = null!;




    [Required(ErrorMessage = "Du måste ange en giltig Email!")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Prompt = "Ange din emailaddress")]
    public string Email { get; set; } = null!;




    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone (optional)", Prompt = "Ange ditt nummer (valfritt)")]
    public string? Phone { get; set; }





    [DataType(DataType.MultilineText)]
    [Display(Name = "Biography (optional)", Prompt = "Add a short bio... (valfritt)")]
    public string? Biography { get; set; }
}

