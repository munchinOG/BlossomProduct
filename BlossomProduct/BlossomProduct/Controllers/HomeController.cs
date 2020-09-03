using BlossomProduct.Core.Models;
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

        public ViewResult Index( )
        {
            var model = _productRepository.GetAllProduct();
            return View( model );
        }

        public ViewResult Details( int? id )
        {
            HomeDetailsVm homeDetailsVm = new HomeDetailsVm()
            {
                Product = _productRepository.GetProduct( id ?? 1 ),
                PageTitle = "Product Details"
            };
            return View( homeDetailsVm );
        }

        [HttpGet]
        public ViewResult Create( )
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create( Product product )
        {
            if(ModelState.IsValid)
            {
                Product newProduct = _productRepository.Add( product );
                return RedirectToAction( "Details", new { id = newProduct.Id } );

            }

            return View();
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
