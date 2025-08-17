using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.db;
using OnlineShop.db.Interfaces;
using OnlineShopWebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area(ConstantFields.AdminRoleName)]
    [Authorize(Roles = ConstantFields.AdminRoleName)]
    public class ProductsController : Controller
    {
        private readonly IProductsRepository productsDbRepository;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment appEnviroment;

        public ProductsController(IProductsRepository productsDbRepository, IMapper mapper, IWebHostEnvironment appEnviroment)
        {
            this.productsDbRepository = productsDbRepository;
            this.mapper = mapper;
            this.appEnviroment = appEnviroment;
        }
        public async Task<IActionResult> ShowProducts()
        {
            var products = await productsDbRepository.GetAllAsync();
            var productsToView = products.Select(product => mapper.Map<ProductViewModel>(product)).ToList() ?? new List<ProductViewModel>();
            return View("Products", productsToView);
        }

        public IActionResult AddProduct()
        {
            return View("ProductInfo");
        }
        [HttpPost]
        public async Task<IActionResult> Save(ProductViewModel product, Guid? id)
        {
            if (!ModelState.IsValid && id != null)
            {
                var productFromRepository = await productsDbRepository.TryGetProductByIdAsync(id.Value);
                return View("ProductInfo", productFromRepository);
            }

            if (!ModelState.IsValid && id == null)
            {
                return View("ProductInfo", product);
            }

            if (product.UploadedFile != null)
            {
                string productImagePath = Path.Combine(appEnviroment.WebRootPath + "/lib/Images/products/");
                if (!Directory.Exists(productImagePath))
                {
                    Directory.CreateDirectory(productImagePath);
                }
                var fileName = Guid.NewGuid() + "." + product.UploadedFile.FileName.Split('.').Last();

                using (var fileStream = new FileStream(productImagePath + fileName, FileMode.Create))
                {
                    product.UploadedFile.CopyTo(fileStream);
                }
                if (product.Images == null)
                {
                    product.Images = new List<Image>();
                }
                product.Images.Add(new Image() { Link = "/lib/Images/products/" + fileName });
            }
            if (id == null)
            {
                productsDbRepository.AddAsync(new Product(product.Name, product.Cost, product.ShortDescription, product.Images));
            }
            else
            {
                productsDbRepository.EditAsync(new Product(product.Name, product.Cost, product.ShortDescription, product.Images), id.Value);
            }
            return RedirectToAction("ShowProducts", "Products");
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Guid id)
        {
            var product = await productsDbRepository.TryGetProductByIdAsync(id);
            var productToView = mapper.Map<ProductViewModel>(product);
            if (product == null)
            {
                return RedirectToAction("ShowProducts", "Products");
            }
            return View("ProductInfo", productToView);
        }
        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            if (id != Guid.Empty)
            {
                productsDbRepository.DeleteAsync(id);
            }
            return RedirectToAction("ShowProducts", "Products");
        }
    }
}