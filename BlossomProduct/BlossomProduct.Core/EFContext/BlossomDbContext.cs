using BlossomProduct.Core.Models;
using BlossomProduct.Core.Models.ModelBuilder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlossomProduct.Core.EFContext
{
    public class BlossomDbContext : IdentityDbContext<ApplicationUser>
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

            foreach(var foreignKey in modelBuilder.Model.GetEntityTypes()
                .SelectMany( e => e.GetForeignKeys() ))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
