using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.Controllers
{
    public class AuthController : Controller
    {


       
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost] //vid submit
        public IActionResult SignUp(SignUpViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
