using System.ComponentModel.DataAnnotations;

namespace BlossomProduct.Core.ViewModels
{
    public class CreateRoleVm
    {
        [Required]
        [Display( Name = "Role" )]
        public string RoleName { get; set; }
    }
}
