using BlossomProduct.Core.Enum;
using System.Collections.Generic;
using System.Linq;

namespace BlossomProduct.Core.Models.Repo
{
    public class MockProductRepository : IProductRepository
    {
        private List<Product> _productList;

        public MockProductRepository( )
        {
            _productList = new List<Product>()
            {
                new Product() {Id = 1, Name = "Blossom Chips", Price = 12.22M, Group = GroupType.Food, ShortDescription = "You will want more.", LongDescription = "It's one of the best chips you can find out now in every shop. "},
                new Product() {Id = 1, Name = "Blossom Body Cream", Price = 22.22M, Group = GroupType.Cosmetics, ShortDescription = "Makes you look good.", LongDescription = "It's one of the best body creams you can find out now in every shop. "},
                new Product() {Id = 1, Name = "Blossom Soap", Price = 12.22M, Group = GroupType.Cosmetics, ShortDescription = "Brings the real you.", LongDescription = "It's one of the best soaps you can find out now in every shop. "},

            };
        }
        public Product GetProduct( int id )
        {
            return _productList.FirstOrDefault( p => p.Id == id );
        }

        public IEnumerable<Product> GetAllProduct( )
        {
            return _productList;
        }

        public Product Add( Product product )
        {
            product.Id = _productList.Max( p => p.Id ) + 1;
            _productList.Add( product );
            return product;
        }

        public Product Update( Product productChanges )
        {
            Product product = _productList.FirstOrDefault( p => p.Id == productChanges.Id );
            if(product != null)
            {
                product.Name = productChanges.Name;
                product.Price = productChanges.Price;
                product.ShortDescription = productChanges.ShortDescription;
                product.LongDescription = productChanges.LongDescription;
                product.Group = productChanges.Group;
            }

            return product;
        }

        public Product Delete( int id )
        {
            Product product = _productList.FirstOrDefault( p => p.Id == id );
            if(product != null)
            {
                _productList.Remove( product );
            }

            return product;
        }
    }
}
