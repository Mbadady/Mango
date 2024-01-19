using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace Mango.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(IProductService productService, ICartService cartService)
    {
        _productService = productService;
        _cartService = cartService;
    }



    public async Task<IActionResult> Index()
    {
        List<ProductDTO?> list = new();

        var response = await _productService.GetAllProductAsync();

        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
        }

        return View(list);
    }

    [Authorize]
    public async Task<IActionResult> ProductDetails(int productId)
    {
        ProductDTO? model = new();

        var response = await _productService.GetProductByIdAsync(productId);

        if (response != null && response.IsSuccess)
        {
           model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
        }
        else
        {
            TempData["error"] = response?.Message;
        }

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ActionName("ProductDetails")]
    public async Task<IActionResult> ProductDetails(ProductDTO productDTO)
    {
        CartDTO cartDTO = new CartDTO()
        {
            CartHeaderDTO = new CartHeaderDTO
            {
                UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault().Value,

            }
        };

        CartDetailsDTO cartDetails = new CartDetailsDTO()
        {
            Count = productDTO.Count,
            ProductId = productDTO.ProductId
        };

        List<CartDetailsDTO> cartDetailsDTOs = new List<CartDetailsDTO>() { cartDetails};

        cartDTO.CartDetailsDTO = cartDetailsDTOs;

        var response = await _cartService.UpsertCartAsync(cartDTO);

        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Item added to the cart";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["error"] = response?.Message;
        }

        return View(productDTO);

    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

