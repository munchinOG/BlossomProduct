using Microsoft.AspNetCore.Mvc;

namespace BlossomProduct.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register( )
        {
            return View();
        }
    }
}
