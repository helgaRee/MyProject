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





    [HttpGet]
    [Route("/account/details")]
    public async Task<IActionResult> Details()
    {
        //if (!_signInManager.IsSignedIn(User))
        //    return RedirectToAction("SignIn", "Auth");

        var userEntity = await _userManager.GetUserAsync(User);

        var viewModel = new AccountDetailsViewModel()
        {
            User = userEntity!

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

