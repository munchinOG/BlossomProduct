using BlossomProduct.Core.EFContext;
using System.Collections.Generic;

namespace BlossomProduct.Core.Models.Repo
{
    public class SqlProductRepository : IProductRepository
    {
        private readonly BlossomDbContext _context;

        public SqlProductRepository( BlossomDbContext context )
        {
            _context = context;
        }
        public Product GetProduct( int id )
        {
            return _context.Products.Find( id );
        }

        public IEnumerable<Product> GetAllProduct( )
        {
            return _context.Products;
        }

        public Product Add( Product product )
        {
            _context.Products.Add( product );
            _context.SaveChanges();
            return product;
        }

        public Product Update( Product productChanges )
        {
            var product = _context.Products.Attach( productChanges );
            product.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return productChanges;
        }

        public Product Delete( int id )
        {
            Product product = _context.Products.Find( id );
            if(product != null)
            {
                _context.Products.Remove( product );
                _context.SaveChanges();
            }

            return product;

        }
    }
}
