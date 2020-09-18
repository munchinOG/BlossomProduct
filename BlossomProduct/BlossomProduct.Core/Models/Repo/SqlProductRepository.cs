using BlossomProduct.Core.EFContext;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace BlossomProduct.Core.Models.Repo
{
    public class SqlProductRepository : IProductRepository
    {
        private readonly BlossomDbContext _context;
        private readonly ILogger<SqlProductRepository> _logger;

        public SqlProductRepository( BlossomDbContext context,
                                    ILogger<SqlProductRepository> logger )
        {
            _context = context;
            _logger = logger;
        }
        public Product GetProduct( int id )
        {
            _logger.LogTrace( "Trace Log" );
            _logger.LogDebug( "Debug Log" );
            _logger.LogInformation( "Information Log" );
            _logger.LogWarning( "Warning Log" );
            _logger.LogError( "Error Log" );
            _logger.LogCritical( "Critical Log" );

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
