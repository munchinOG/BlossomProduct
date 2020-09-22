using BlossomProduct.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlossomProduct.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministrationController( RoleManager<IdentityRole> roleManager )
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult CreateRole( )
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole( CreateRoleVm model )
        {
            if(ModelState.IsValid)
            {
                // We just need to specify a unique role name to create a new role
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                // Saves the role in the underlying AspNetRoles table
                IdentityResult result = await _roleManager.CreateAsync( identityRole );

                if(result.Succeeded)
                {
                    return RedirectToAction( "Index", "Home" );
                }

                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError( "", error.Description );
                }
            }

            return View( model );
        }
    }
}
