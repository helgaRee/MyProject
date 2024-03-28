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


    public AccountController(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, AccountService accountService = null)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _accountService = accountService;
    }




    #region DETAILS
    [HttpGet]
    [Route("/account/details")]
    public async Task<IActionResult> Details()
    {
        //if (!_signInManager.IsSignedIn(User))
        //    return RedirectToAction("SignIn", "Auth");

        //var userEntity = await _userManager.GetUserAsync(User);

        var viewModel = new AccountDetailsViewModel
        {
            BasicInfoForm = await PopulateBasicInfoFormAsync()

        };
        return View(viewModel);


        //var viewModel = new AccountDetailsViewModel();

        //viewModel.ProfileInfo = await PopulateProfileInfoAsync();
        //viewModel.BasicInfoForm ??= await PopulateBasicInfoAsync();
        //viewModel.AddressInfoForm ??= await PopulateAddressInfoAsync();

        //return View(viewModel);

    }
    #endregion


    #region BASICINFO

    [HttpPost]
    [Route("/account/details")]
    public async Task<IActionResult> BasicInfo(AccountDetailsViewModel viewModel)
    {
        if (viewModel.User != null)
        {
            var result = await _userManager.UpdateAsync(viewModel.User);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Failed to save data", "Unable to save the data");
                ViewData["ErrorMessage"] = "Unable to save the data";
            }
        }
        return RedirectToAction("Details", "Account", viewModel);



    }
    #endregion

    #region ADDRESSINFO

    [HttpPost]
    [Route("/account/details")]
    public async Task<IActionResult> AddressInfo(AccountDetailsViewModel viewModel)
    {
        if (viewModel.User != null)
        {
            var result = await _userManager.UpdateAsync(viewModel.User);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Failed to save data", "Unable to save the data");
                ViewData["ErrorMessage"] = "Unable to save the data";
            }
        }
        return RedirectToAction("Details", "Account", viewModel);

    }
    #endregion
    private async Task<BasicInfoFormViewModel> PopulateBasicInfoFormAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            return new BasicInfoFormViewModel
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Phone = user.Phone,
                Biography = user.Biography,
            };
        }
        return null!;
    }
}

