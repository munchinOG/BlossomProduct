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
                string uniqueFileName = ProcessUploadedFile( model );
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

        [HttpPost]
        public IActionResult Edit( ProductEditVm model )
        {
            if(ModelState.IsValid)
            {
                Product product = _productRepository.GetProduct( model.Id );
                product.Name = model.Name;
                product.Price = model.Price;
                product.ShortDescription = model.ShortDescription;
                product.LongDescription = model.LongDescription;
                if(model.Photo != null)
                {
                    if(model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine( _webHostEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath );
                        System.IO.File.Delete( filePath );
                    }
                    product.PhotoPath = ProcessUploadedFile( model );
                }

                var updatedProduct = _productRepository.Update( product );
                return RedirectToAction( "Index" );
            }

            return View( model );
        }

        private string ProcessUploadedFile( ProductCreateVm model )
        {
            string uniqueFileName = null;
            if(model.Photo != null)
            {
                string uploadsFolder = Path.Combine( _webHostEnvironment.WebRootPath, "images" );
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine( uploadsFolder, uniqueFileName );
                using(var fileStream = new FileStream( filePath, FileMode.Create ))
                {
                    model.Photo.CopyTo( fileStream );

                }
            }

            return uniqueFileName;
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
