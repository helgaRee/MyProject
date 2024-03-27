using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class DefaultController : Controller
    {
        [Route("/")]
        public IActionResult Index() => View();

        [Route("/error")]
        public IActionResult Error404(int statusCode)
        {
            return View();
        }
    }
}
