using BlossomProduct.Core.Enum;

namespace BlossomProduct.Core.Models.ModelBuilder
{
    public static class ModelBuilderExtensions
    {
        public static void Seed( this Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder )
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 101,
                    Name = "Body Cream",
                    Group = GroupType.Cosmetics,
                    Price = 20.00M,
                    ShortDescription = "Nice to the skin.",
                    LongDescription = "You will love it, plus it brighten your skin."
                },
                new Product
                {
                    Id = 102,
                    Name = "Chin-Chin",
                    Group = GroupType.Food,
                    Price = 5.00M,
                    ShortDescription = " Very nice and sweet.",
                    LongDescription = "You will love to eat it all day long."
                }
            );
        }
    }
}
