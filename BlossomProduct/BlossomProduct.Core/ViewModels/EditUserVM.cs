﻿using BlossomProduct.Core.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlossomProduct.Core.ViewModels
{
    public class EditUserVM
    {
        public EditUserVM( )
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public GenderType Gender { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}
