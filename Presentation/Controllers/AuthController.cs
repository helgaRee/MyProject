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

        public AuthController(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost] //vid submit
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


    }
}
