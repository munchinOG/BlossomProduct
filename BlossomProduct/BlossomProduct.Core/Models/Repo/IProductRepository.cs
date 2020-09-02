using System.Collections.Generic;

namespace BlossomProduct.Core.Models.Repo
{
    public interface IProductRepository
    {
        Product GetProduct( int Id );
        IEnumerable<Product> GetAllProduct( );
    }
}
