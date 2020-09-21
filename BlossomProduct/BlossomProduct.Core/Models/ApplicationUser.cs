using BlossomProduct.Core.Enum;
using Microsoft.AspNetCore.Identity;

namespace BlossomProduct.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
        public string Address { get; set; }
        public GenderType Gender { get; set; }
    }
}
