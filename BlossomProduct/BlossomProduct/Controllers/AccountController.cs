using BlossomProduct.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlossomProduct.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManger;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController( UserManager<IdentityUser> userManger,
                                 SignInManager<IdentityUser> signInManager )
        {
            _userManger = userManger;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Logout( )
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction( "Index", "Home" );
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register( )
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register( RegisterVm model )
        {
            if(ModelState.IsValid)
            {
                // Copy data from RegisterViewModel to IdentityUser
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                // Store user data in AspNetUsers database table
                var result = await _userManger.CreateAsync( user, model.Password );

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync( user, isPersistent: false );
                    return RedirectToAction( "Index", "Home" );
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError( string.Empty, error.Description );
                }
            }

            return View( model );
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login( )
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login( LoginVm model )
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, false );

                if(result.Succeeded)
                {
                    return RedirectToAction( "index", "home" );
                }

                ModelState.AddModelError( string.Empty, "Invalid Login Attempt" );
            }

            return View( model );
        }
    }
}
