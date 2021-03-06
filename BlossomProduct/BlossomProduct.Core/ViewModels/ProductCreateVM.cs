﻿using BlossomProduct.Core.Enum;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlossomProduct.Core.ViewModels
{
    public class ProductCreateVm
    {
        [Required]
        [MaxLength( 30, ErrorMessage = "Name cannot exceed 15 Characters" )]
        public string Name { get; set; }

        [Required]
        public GroupType? Group { get; set; }

        [Required]
        [MaxLength( 100, ErrorMessage = " Short Description cannot exceed 20 Characters" )]
        public string ShortDescription { get; set; }

        [Required]
        [MaxLength( 200, ErrorMessage = "Long Description cannot exceed 200 Characters" )]
        public string LongDescription { get; set; }

        [Required]
        public decimal Price { get; set; }

        public IFormFile Photo { get; set; }
    }
}
