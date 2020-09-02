using BlossomProduct.Core.Models.Repo;
using BlossomProduct.Core.ViewModels;
using BlossomProduct.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BlossomProduct.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController( IProductRepository productRepository, ILogger<HomeController> logger )
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public IActionResult Index( )
        {
            var model = _productRepository.GetAllProduct();
            return View( model );
        }

        public IActionResult Details( int id )
        {
            HomeDetailsVM homeDetailsVm = new HomeDetailsVM()
            {
                Product = _productRepository.GetProduct( id ),
                PageTitle = "Product Details"
            };
            return View( homeDetailsVm );
        }

        public IActionResult Privacy( )
        {
            return View();
        }

        [ResponseCache( Duration = 0, Location = ResponseCacheLocation.None, NoStore = true )]
        public IActionResult Error( )
        {
            return View( new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } );
        }
    }
}
