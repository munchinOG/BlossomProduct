using System.Collections.Generic;

namespace BlossomProduct.Core.ViewModels
{
    public class UserClaimsVM
    {
        public UserClaimsVM( )
        {
            Cliams = new List<UserClaim>();
        }

        public string UserId { get; set; }
        public List<UserClaim> Cliams { get; set; }
    }
}
