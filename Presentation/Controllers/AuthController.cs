using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.ViewModels;

namespace Presentation.Controllers
{
    public class AuthController : Controller
    {

        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;

        public AuthController(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region SIGN UP
        [HttpGet]
        [Route("/signup")]
        public IActionResult SignUp()
        {
            return View();
        }


        [HttpPost] //vid submit
        [Route("/signup")]
        public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var exists = await _userManager.Users.AnyAsync(x => x.Email == viewModel.Email);

                if (exists)
                {
                    ModelState.AddModelError("Finns redan", "Användaren finns redan!");
                    ViewData["ErrorMessage"] = "användaren med den meailen finns redan juu";
                    return View(viewModel);
                }
                //factory?
                var userEntity = new UserEntity()
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Email = viewModel.Email,
                    UserName = viewModel.Email
                };

                var result = await _userManager.CreateAsync(userEntity, viewModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("SignIn", "Auth");
                }
            }
            return View(viewModel);
        }

        #endregion


        [HttpGet]
        [Route("/signin")]
        public IActionResult SignIn(string returnUrl)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Details", "Account");
            }

            ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
            return View();
        }

        [HttpPost]
        [Route("/signin")]
        public async Task<IActionResult> SignIn(SignInViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return RedirectToAction(returnUrl);

                    return RedirectToAction("Details", "Account");
                }
            }

            //om jej giltig
            ModelState.AddModelError("fel värden", "fel email eller lösenord");
            ViewData["ErrorMessage"] = "fel fel fel email eller lösen!";
            return View(viewModel);
        }



    }
}
