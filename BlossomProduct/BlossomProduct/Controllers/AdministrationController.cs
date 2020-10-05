﻿using BlossomProduct.Core.Models;
using BlossomProduct.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlossomProduct.Controllers
{
    [Authorize( Roles = "Admin" )]
    //[Authorize( Roles = "User" )]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AdministrationController> _logger;

        public AdministrationController( RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager,
                                        ILogger<AdministrationController> logger )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser( string id )
        {
            var user = await _userManager.FindByIdAsync( id );

            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View( "NotFound" );
            }
            else
            {
                var result = await _userManager.DeleteAsync( user );

                if(result.Succeeded)
                {
                    return RedirectToAction( "ListUsers" );
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError( "", error.Description );
                }

                return View( "ListUsers" );
            }
        }

        [HttpPost]
        //[Authorize( Policy = "DeleteRolePolicy" )]
        public async Task<IActionResult> DeleteRole( string id )
        {
            var role = await _roleManager.FindByIdAsync( id );

            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View( "NotFound" );
            }
            else
            {
                try
                {
                    //throw new Exception("Test Exception");

                    var result = await _roleManager.DeleteAsync( role );

                    if(result.Succeeded)
                    {
                        return RedirectToAction( "ListRoles" );
                    }

                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError( "", error.Description );
                    }

                    return View( "ListRoles" );
                }
                catch(DbUpdateException ex)
                {
                    _logger.LogError( $"Error deleting role {ex}" );

                    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users " +
                        $"in this role. If you want to delete this role, please remove the users from " +
                        $"the role and then try to delete";
                    return View( "Error" );
                }
            }
        }

        [HttpGet]
        public IActionResult ListUsers( )
        {
            var users = _userManager.Users;
            return View( users );
        }

        [HttpGet]
        public async Task<IActionResult> EditUser( string id )
        {
            var user = await _userManager.FindByIdAsync( id );

            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View( "NotFound" );
            }

            var userClaims = await _userManager.GetClaimsAsync( user );
            var userRoles = await _userManager.GetRolesAsync( user );

            var model = new EditUserVM
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Gender = user.Gender,
                Address = user.Address,
                City = user.City,
                Claims = userClaims.Select( c => c.Type + " : " + c.Value ).ToList(),
                Roles = userRoles
            };

            return View( model );
        }

        [HttpPost]
        public async Task<IActionResult> EditUser( EditUserVM model )
        {
            var user = await _userManager.FindByIdAsync( model.Id );

            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View( "NotFound" );
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.City = model.City;
                user.Address = model.Address;
                user.Gender = model.Gender;

                var result = await _userManager.UpdateAsync( user );

                if(result.Succeeded)
                {
                    return RedirectToAction( "ListUsers" );
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError( "", error.Description );
                }

                return View( model );
            }
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
                    return RedirectToAction( "ListRoles", "Administration" );
                }

                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError( "", error.Description );
                }
            }

            return View( model );
        }

        [HttpGet]
        public IActionResult ListRoles( )
        {
            var roles = _roleManager.Roles;
            return View( roles );
        }

        // Role ID is passed from the URL to the action
        [HttpGet]
        public async Task<IActionResult> EditRole( string id )
        {
            // Find the role by Role ID
            var role = await _roleManager.FindByIdAsync( id );

            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View( "NotFound" );
            }

            var model = new EditRoleVm
            {
                Id = role.Id,
                RoleName = role.Name
            };

            // Retrieve all the Users
            foreach(var user in _userManager.Users)
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. This model
                // object is then passed to the view for display
                if(await _userManager.IsInRoleAsync( user, role.Name ))
                {
                    model.Users.Add( user.UserName );
                }
            }

            return View( model );
        }

        // This action responds to HttpPost and receives EditRoleViewModel
        [HttpPost]
        public async Task<IActionResult> EditRole( EditRoleVm model )
        {
            var role = await _roleManager.FindByIdAsync( model.Id );

            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View( "NotFound" );
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await _roleManager.UpdateAsync( role );

                if(result.Succeeded)
                {
                    return RedirectToAction( "ListRoles" );
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError( "", error.Description );
                }

                return View( model );
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole( string roleId )
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync( roleId );

            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View( "NotFound" );
            }

            var model = new List<UserRoleVM>();

            foreach(var user in _userManager.Users)
            {
                var userRoldVM = new UserRoleVM
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if(await _userManager.IsInRoleAsync( user, role.Name ))
                {
                    userRoldVM.IsSelected = true;
                }
                else
                {
                    userRoldVM.IsSelected = false;
                }

                model.Add( userRoldVM );
            }

            return View( model );
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole( List<UserRoleVM> model, string roleId )
        {
            var role = await _roleManager.FindByIdAsync( roleId );

            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View( "NotFound" );
            }

            for(int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync( model[i].UserId );

                IdentityResult result = null;

                if(model[i].IsSelected && !(await _userManager.IsInRoleAsync( user, role.Name )))
                {
                    result = await _userManager.AddToRoleAsync( user, role.Name );
                }
                else if(!model[i].IsSelected && await _userManager.IsInRoleAsync( user, role.Name ))
                {
                    result = await _userManager.RemoveFromRoleAsync( user, role.Name );
                }
                else
                {
                    continue;
                }

                if(result.Succeeded)
                {
                    if(i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction( "EditRole", new { Id = roleId } );
                }
            }

            return RedirectToAction( "EditRole", new { Id = roleId } );
        }
    }
}
