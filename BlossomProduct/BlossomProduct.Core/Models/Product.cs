using BlossomProduct.Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace BlossomProduct.Core.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength( 500, ErrorMessage = "Name cannot exceed 500 Characters" )]
        public string Name { get; set; }

        [Required]
        public GroupType? Group { get; set; }

        [Required]
        [MaxLength( 2000, ErrorMessage = " Short Description cannot exceed 1000 Characters" )]
        public string ShortDescription { get; set; }

        [Required]
        [MaxLength( 5000, ErrorMessage = "Long Description cannot exceed 3000 Characters" )]
        public string LongDescription { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string PhotoPath { get; set; }
    }
}
