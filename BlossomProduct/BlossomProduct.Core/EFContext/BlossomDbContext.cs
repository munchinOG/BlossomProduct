using BlossomProduct.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BlossomProduct.Core.EFContext
{
    public class BlossomDbContext : DbContext
    {
        public BlossomDbContext( DbContextOptions<BlossomDbContext> options )
        : base( options )
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
