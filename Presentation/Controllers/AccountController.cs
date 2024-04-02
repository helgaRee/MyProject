using Infrastructure.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels.Account;

namespace Presentation.Controllers;


[Authorize]
public class AccountController : Controller
{

    private readonly SignInManager<UserEntity> _signInManager;
    private readonly UserManager<UserEntity> _userManager;
    private readonly AccountService _accountService;
    private readonly AddressService _addressService;

    public AccountController(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, AccountService accountService, AddressService addressService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _accountService = accountService;
        _addressService = addressService;
    }







    #region Details
    [HttpGet]
    [Route("/account/details")]
    public async Task<IActionResult> Details()
    {
        var viewModel = new AccountDetailsViewModel();

        viewModel.ProfileInfo = await PopulateProfileInfoAsync();
        viewModel.BasicInfoForm ??= await PopulateBasicInfoAsync();
        viewModel.AddressInfoForm ??= await PopulateAddressInfoAsync();

        return View(viewModel);
    }



    [HttpPost]
    [Route("/account/details")]
    public async Task<IActionResult> Details(AccountDetailsViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        if (viewModel.BasicInfoForm != null)
        {
            if (
                viewModel.BasicInfoForm.FirstName != null &&
                viewModel.BasicInfoForm.LastName != null &&
                viewModel.BasicInfoForm.Email != null
                )
            {
                var user = await _userManager.GetUserAsync(User);

                //OM USER FINNS, UPPDATERA
                if (user != null)
                {
                    user.FirstName = viewModel.BasicInfoForm.FirstName;
                    user.LastName = viewModel.BasicInfoForm.LastName;
                    user.Email = viewModel.BasicInfoForm.Email;
                    user.PhoneNumber = viewModel.BasicInfoForm.Phone;
                    user.Biography = viewModel.BasicInfoForm.Biography;

                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {//felmeddelanden om input är inkrrekt
                        ModelState.AddModelError("Incorrect values", "Something went wrong! Unable to save data.");
                        ViewData["ErrorMessage"] = "Something went wrong! Unable to update basic information data.";
                    }
                }
            }
        }

        //UPPDATERA ADDRESSINFO

        if (viewModel.AddressInfoForm != null)
        {


            if (viewModel.AddressInfoForm.AddressLine_1 != null && viewModel.AddressInfoForm.AddressLine_2 != null && viewModel.AddressInfoForm.PostalCode != null && viewModel.AddressInfoForm.City != null)
            {

                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    var address = await _addressService.GetAddressAsync(user.Id);
                    //om addressen inte är null, uppdatera
                    if (address != null)
                    {
                        address.AddressLine1 = viewModel.AddressInfoForm.AddressLine_1;
                        address.AddressLine2 = viewModel.AddressInfoForm.AddressLine_2;
                        address.PostalCode = viewModel.AddressInfoForm.PostalCode;
                        address.City = viewModel.AddressInfoForm.City;

                        var updatedAddress = await _addressService.UpdateAddressAsync(address);
                        if (!updatedAddress)
                        {
                            ModelState.AddModelError("Ogiltiga värden", "Något gick fel!");
                            ViewData["ErrorMessage"] = "Något gick fel! Det gick inte att uppdatera addressinfo, detta är ur 'ViewData'";
                        }
                    }
                    //om addressen ÄR null, SKAPA en ny
                    else
                    {
                        address = new AddressEntity
                        {
                            Users.Id = user.Id,
                            AddressLine1 = viewModel.AddressInfoForm.AddressLine_1,
                            AddressLine2 = viewModel.AddressInfoForm.AddressLine_2,
                            PostalCode = viewModel.AddressInfoForm.PostalCode,
                            City = viewModel.AddressInfoForm.City,

                        };

                        var newAddress = await _addressService.GetAddressAsync(address);
                        if (!newAddress)
                        {
                            ModelState.AddModelError("Ogiltiga värden", "Något gick fel!");
                            ViewData["ErrorMessage"] = "Något gick fel! Det gick inte att uppdatera addressinfo, detta är ur 'ViewData'";
                        }
                    }
                }



                var result = await _userManager.UpdateAsync(user!);
                if (!result.Succeeded)
                {
                    //skriv ut errors
                    ModelState.AddModelError("Ogiltiga värden", "Något gick fel!");
                    ViewData["ErrorMessage"] = "Något gick fel! Det gick inte att uppdatera, detta är ur 'ViewData'";
                }
            }
        }



        //oavsettvad ska alltid profilinofmraitonen hämtas
        viewModel.ProfileInfo = await PopulateProfileInfoAsync();
        //Hämtar den NYA informationen och poppulerar den
        viewModel.BasicInfoForm ??= await PopulateBasicInfoAsync();
        viewModel.AddressInfoForm ??= await PopulateAddressInfoAsync();

        return View(viewModel);

    }

    #endregion


    private async Task<ProfileInfoViewModel> PopulateProfileInfoAsync()
    {
        //HÄMTAR ANVÄNDARINFORMATIONEN BASERAT PÅ CLAIMS
        var user = await _userManager.GetUserAsync(User);
        //kontrollera om null först
        if (user != null)
        {
            return new ProfileInfoViewModel
            {
                FirstName = user!.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                //bilden är hårdkådad nu, finns ej i db
            };

        }
        return null!;
    }


    //metod somreturnerar information ur basicinfoform. Gör att information om användaren hämtas till Details-sidan.
    //returnerar en AccountDetailsViewModel
    private async Task<BasicInfoFormViewModel> PopulateBasicInfoAsync()
    {
        //HÄMTAR ANVÄNDARINFORMATIONEN BASERAT PÅ CLAIMS
        var user = await _userManager.GetUserAsync(User);

        if (user != null)
        {
            return new BasicInfoFormViewModel
            {
                UserId = user!.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Biography = user.Biography,
                Phone = user.PhoneNumber,
            };
        }
        else
        {
            // Hantera fall där användaren är null (t.ex. logga fel eller returnera en tom modell)
            // Här returnerar jag en tom modell som exempel, men du kan ändra detta efter behov
            return new BasicInfoFormViewModel();
        }
    }


    private async Task<AddressInfoFormViewModel> PopulateAddressInfoAsync()
    {

        //Returnerar en tom model
        return new AddressInfoFormViewModel();
    }
}