using System.Collections.Generic;

namespace BlossomProduct.Core.Models.Repo
{
    public interface IProductRepository
    {
        Product GetProduct( int id );
        IEnumerable<Product> GetAllProduct( );
        Product Add( Product product );
        Product Update( Product productChanges );
        Product Delete( int id );
    }
}
