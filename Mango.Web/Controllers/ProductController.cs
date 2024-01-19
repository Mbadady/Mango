using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Web.Controllers
{
    
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
   
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO?> list = new();

            var response = await _productService.GetAllProductAsync();

            if(response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProductAsync(productDTO);

                if(response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                
            }
            else
            {
                // Log or inspect the validation errors
                foreach (var modelStateEntry in ModelState)
                {
                    var propertyName = modelStateEntry.Key;
                    var errors = modelStateEntry.Value.Errors;
                    foreach (var error in errors)
                    {
                        // Log or inspect the specific validation errors
                        // You can access error.ErrorMessage or error.Exception
                       var message =  error.ErrorMessage;
                    }
                }
            }

            return View(productDTO);
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            var response = await _productService.GetProductByIdAsync(productId);

            if(response != null && response.IsSuccess)
            {
              var product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                
                return View(product);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDTO productDTO)
        {
            var response = await _productService.DeleteProductAsync(productDTO.ProductId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";

                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDTO);

        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            var response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                var product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));

                return View(product);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(productDTO);
            }
            var response = await _productService.UpdateProductAsync(productDTO);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product Updated successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDTO);

        }
    }
}

