using BlossomProduct.Core.Models;
using BlossomProduct.Core.Models.ModelBuilder;
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
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            modelBuilder.Seed();
            //base.OnModelCreating(modelBuilder);
        }
    }
}
