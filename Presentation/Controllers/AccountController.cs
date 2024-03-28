using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels.Account;

namespace Presentation.Controllers;

public class AccountController : Controller
{

    private readonly SignInManager<UserEntity> _signInManager;
    private readonly UserManager<UserEntity> _userManager;

    public AccountController(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }





    [HttpGet]
    [Route("/account/details")]
    public async Task<IActionResult> Details()
    {
        if (!_signInManager.IsSignedIn(User))
            return RedirectToAction("SignIn", "Auth");

        var viewModel = new AccountDetailsViewModel()
        {
            //BasicInfo = await PopulateBasicInfo()
        };
        return View(viewModel);

    }

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


}

