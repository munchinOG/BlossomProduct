using BlossomProduct.Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlossomProduct.Core.ViewModels
{
    public class RegisterVm
    {
        [Required]
        [EmailAddress]
        [Remote( action: "IsEmailInUse", controller: "Account" )]
        [ValidEmailDomain( allowedDomain: "blossomorganics.com",
            ErrorMessage = "Email domain must be blossomorganics.com" )]
        public string Email { get; set; }

        [Required]
        [DataType( DataType.Password )]
        public string Password { get; set; }

        [DataType( DataType.Password )]
        [Display( Name = "Confirm password" )]
        [Compare( "Password",
            ErrorMessage = "Password and confirmation password do not match." )]
        public string ConfirmPassword { get; set; }
    }
}
