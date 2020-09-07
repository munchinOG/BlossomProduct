using BlossomProduct.Core.Models.Repo;
using BlossomProduct.Core.ViewModels;
using BlossomProduct.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using Product = BlossomProduct.Core.Models.Product;

namespace BlossomProduct.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;

        public HomeController( IProductRepository productRepository, IWebHostEnvironment webHostEnvironment,
            ILogger<HomeController> logger )
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult Create( ProductCreateVm model )
        {
            if(ModelState.IsValid)
            {
                string uniqueFileName = null;
                if(model.Photo != null)
                {
                    string uploadFolder = Path.Combine( _webHostEnvironment.WebRootPath, "images" );
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine( uploadFolder, uniqueFileName );
                    model.Photo.CopyTo( new FileStream( filePath, FileMode.Create ) );

                }
                Product newProduct = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    Group = model.Group,
                    ShortDescription = model.ShortDescription,
                    LongDescription = model.ShortDescription,
                    PhotoPath = uniqueFileName
                };

                _productRepository.Add( newProduct );
                return RedirectToAction( "Details", new { id = newProduct.Id } );
            }

            return View();
        }

        [HttpGet]
        public ViewResult Edit( int id )
        {
            Product product = _productRepository.GetProduct( id );
            ProductEditVm productEditVm = new ProductEditVm
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ShortDescription = product.ShortDescription,
                LongDescription = product.LongDescription,
                ExistingPhotoPath = product.PhotoPath
            };
            return View( productEditVm );
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
