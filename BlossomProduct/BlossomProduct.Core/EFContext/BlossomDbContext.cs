using BlossomProduct.Core.Models;
using BlossomProduct.Core.Models.ModelBuilder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlossomProduct.Core.EFContext
{
    public class BlossomDbContext : IdentityDbContext
    {
        public BlossomDbContext( DbContextOptions<BlossomDbContext> options )
        : base( options )
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );
            modelBuilder.Seed();
        }
    }
}
