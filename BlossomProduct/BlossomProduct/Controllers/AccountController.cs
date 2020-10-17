using BlossomProduct.Core.Models;
using BlossomProduct.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlossomProduct.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController( UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager )
        {
            _userManager = userManager;
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
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Address = model.Address,
                    City = model.City,
                    Gender = model.Gender
                };

                // Store user data in AspNetUsers database table
                var result = await _userManager.CreateAsync( user, model.Password );

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if(result.Succeeded)
                {
                    if(_signInManager.IsSignedIn( User ) && User.IsInRole( "Admin" ))
                    {
                        return RedirectToAction( "ListUsers", "Administration" );
                    }

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
        public async Task<IActionResult> Login( string returnUrl )
        {
            LoginVm model = new LoginVm
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View( model );
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login( LoginVm model, string returnUrl )
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, false );

                if(result.Succeeded)
                {
                    if(!string.IsNullOrEmpty( returnUrl ) && Url.IsLocalUrl( returnUrl ))
                    {
                        return Redirect( returnUrl );
                    }
                    else
                    {
                        return RedirectToAction( "index", "home" );
                    }
                }

                ModelState.AddModelError( string.Empty, "Invalid Login Attempt" );
            }

            return View( model );
        }

        [AcceptVerbs( "Get", "Post" )]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse( string email )
        {
            var user = await _userManager.FindByEmailAsync( email );

            if(user == null)
            {
                return Json( true );
            }
            else
            {
                return Json( $"Email {email} is already in use." );
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin( string provider, string returnUrl )
        {
            var redirectUrl = Url.Action( "ExternalLoginCallback", "Account",
                                new { ReturnUrl = returnUrl } );
            var properties = _signInManager
                .ConfigureExternalAuthenticationProperties( provider, redirectUrl );
            return new ChallengeResult( provider, properties );
        }

        [AllowAnonymous]
        public async Task<IActionResult>
            ExternalLoginCallback( string returnUrl = null, string remoteError = null )
        {
            returnUrl = returnUrl ?? Url.Content( "~/" );

            LoginVm loginViewModel = new LoginVm
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                        (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if(remoteError != null)
            {
                ModelState
                    .AddModelError( string.Empty, $"Error from external provider: {remoteError}" );

                return View( "Login", loginViewModel );
            }

            // Get the login information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if(info == null)
            {
                ModelState
                    .AddModelError( string.Empty, "Error loading external login information." );

                return View( "Login", loginViewModel );
            }

            // If the user already has a login (i.e if there is a record in AspNetUserLogins
            // table) then sign-in the user with this external login provider
            var signInResult = await _signInManager.ExternalLoginSignInAsync( info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true );

            if(signInResult.Succeeded)
            {
                return LocalRedirect( returnUrl );
            }
            // If there is no record in AspNetUserLogins table, the user may not have
            // a local account
            else
            {
                // Get the email claim value
                var email = info.Principal.FindFirstValue( ClaimTypes.Email );

                if(email != null)
                {
                    // Create a new user without password if we do not have a user already
                    var user = await _userManager.FindByEmailAsync( email );

                    if(user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue( ClaimTypes.Email ),
                            Email = info.Principal.FindFirstValue( ClaimTypes.Email )
                        };

                        await _userManager.CreateAsync( user );
                    }

                    // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                    await _userManager.AddLoginAsync( user, info );
                    await _signInManager.SignInAsync( user, isPersistent: false );

                    return LocalRedirect( returnUrl );
                }

                // If we cannot find the user email we cannot continue
                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on SuperAdmin@BlossomOrganics.com";

                return View( "Error" );
            }
        }
    }
}
