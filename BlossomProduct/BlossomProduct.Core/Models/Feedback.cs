using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace BlossomProduct.Core.Models
{
    public class Feedback
    {
        [BindNever]
        public int FeedbackId { get; set; }

        [Required]
        [StringLength( 100, ErrorMessage = "Your name is required" )]
        public string Name { get; set; }

        [Required]
        [StringLength( 100, ErrorMessage = "Your email is required" )]
        [DataType( DataType.EmailAddress )]
        [RegularExpression( @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid Email Format" )]
        public string Email { get; set; }

        [Required]
        [StringLength( 5000, ErrorMessage = "Your message is required" )]
        public string Message { get; set; }
        public bool ContactMe { get; set; }
    }
}
