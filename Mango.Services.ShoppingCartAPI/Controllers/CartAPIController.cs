using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using AutoMapper;
using Mango.MessageBus;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Repository.IRepository;
using Mango.Services.ShoppingCartAPI.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly ICartDetails _cartDetails;
        private readonly ICartHeader _cartHeader;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        protected ResponseDTO _responseDTO;

        public CartAPIController(ICartDetails cartDetails, ICartHeader cartHeader, IMapper mapper,
            IProductService productService, ICouponService couponService, IMessageBus messageBus, IConfiguration configuration)
        {
            _cartDetails = cartDetails;
            _cartHeader = cartHeader;
            _mapper = mapper;
            _productService = productService;
            _couponService = couponService;
            _messageBus = messageBus;
            _configuration = configuration;
            _responseDTO = new ResponseDTO();
        }


        [HttpGet("GetCart/{userId}")]
        public async Task<ActionResult<ResponseDTO>> GetCart(string userId)
        {
            try
            {
                CartDTO cart = new CartDTO
                {
                    CartHeaderDTO = _mapper.Map<CartHeaderDTO>(_cartHeader.GetCartHeaderAsync(u => u.UserId == userId))
                };

                cart.CartDetailsDTO = _mapper.Map<IEnumerable<CartDetailsDTO>>(_cartDetails.GetCartDetailsAsync(u => u.CartHeaderId == cart.CartHeaderDTO.CartHeaderId));

                IEnumerable<ProductDTO> productDTOs = await _productService.GetProducts();

                foreach(var item in cart.CartDetailsDTO)
                {
                    item.ProductDTO = productDTOs.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cart.CartHeaderDTO.CartTotal = cart.CartHeaderDTO.CartTotal + (item.Count * item.ProductDTO.Price);
                }

                if (!string.IsNullOrEmpty(cart.CartHeaderDTO.CouponCode))
                {
                    var couponDto = await _couponService.GetCouponAsync(cart.CartHeaderDTO.CouponCode);

                    if(couponDto != null && cart.CartHeaderDTO.CartTotal > couponDto.MinAmount)
                    {
                        cart.CartHeaderDTO.CartTotal -= couponDto.DiscountAmount;

                        cart.CartHeaderDTO.Discount = couponDto.DiscountAmount;
                    }
                }

                _responseDTO.Result = cart;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;

            }

            return _responseDTO;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<ActionResult<ResponseDTO>> ApplyCoupon([FromBody] CartDTO cartDTO)
        {
            try
            {
                var cartFromDb = await _cartHeader.GetCartHeaderAsync(u => u.UserId == cartDTO.CartHeaderDTO.UserId);

                cartFromDb.CouponCode = cartDTO.CartHeaderDTO.CouponCode;

                await _cartHeader.UpdateCartHeaderAsync(cartFromDb);

                _responseDTO.Result = true;

                return _responseDTO;

            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
            }
            return _responseDTO;
        }

        [HttpPost("EmailCartRequest")]
        public async Task<ActionResult<ResponseDTO>> EmailCartRequest([FromBody] CartDTO cartDTO)
        {
            try
            {
                await _messageBus.PublishMessage(cartDTO, _configuration["TopicAndQueueNames:EmailShoppingCartQueue"], _configuration.GetValue<string>("EmailShoppingCartConnectionString:ConnectionString"));
                _responseDTO.Result = true;

                return _responseDTO;

            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
            }
            return _responseDTO;
        }

        //[HttpPost("RemoveCoupon")]
        //public async Task<ActionResult<ResponseDTO>> RemoveCoupon([FromBody] CartDTO cartDTO)
        //{
        //    try
        //    {
        //        var cartFromDb = await _cartHeader.GetCartHeaderAsync(u => u.UserId == cartDTO.CartHeaderDTO.UserId);

        //        cartFromDb.CouponCode = "";

        //        await _cartHeader.UpdateCartHeaderAsync(cartFromDb);

        //        _responseDTO.Result = true;

        //        return _responseDTO;

        //    }
        //    catch (Exception ex)
        //    {
        //        _responseDTO.Message = ex.Message;
        //        _responseDTO.IsSuccess = false;
        //    }
        //    return _responseDTO;
        //}


        [HttpPost("UpsertCart")]
        public async Task<ActionResult<ResponseDTO>> CartUpsert([FromBody]CartDTO cartDTO)
        {
            try
            {
                var cartHeaderFromDb = await _cartHeader.GetCartHeaderAsync(u => u.UserId == cartDTO.CartHeaderDTO.UserId);

                if(cartHeaderFromDb == null)
                {
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDTO.CartHeaderDTO);
                    await _cartHeader.CreateHeaderAsync(cartHeader);

                    cartDTO.CartDetailsDTO.First().CartHeaderId = cartHeader.CartHeaderId;

                    await _cartDetails.CreateCartDetailsAsync(_mapper.Map<CartDetails>(cartDTO.CartDetailsDTO));
                }
                else
                {
                    // if header is not null
                    // check cartDetails has same product
                    var cartDetailsFromDb = await _cartDetails.GetCartDetailsAsync(u => u.ProductId == cartDTO.CartDetailsDTO.First().ProductId
                                                                                    && u.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                    if(cartDetailsFromDb == null)
                    {
                        // create cartDetails
                        cartDTO.CartDetailsDTO.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;

                        await _cartDetails.CreateCartDetailsAsync(_mapper.Map<CartDetails>(cartDTO.CartDetailsDTO));
                    }
                    else
                    {
                        // Update the details count
                        cartDTO.CartDetailsDTO.First().Count += cartDetailsFromDb.Count;
                        cartDTO.CartDetailsDTO.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDTO.CartDetailsDTO.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;

                        await _cartDetails.UpdateCartDetailsAsync(_mapper.Map<CartDetails>(cartDTO.CartDetailsDTO));
                    }
                }
                _responseDTO.Result = cartDTO;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;

            }
            return _responseDTO;
        }


        [HttpPost("RemoveCart")]
        public async Task<ActionResult<ResponseDTO>> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                var cartDetailsFromDb = await _cartDetails.GetCartDetailsAsync(u => u.CartDetailsId == cartDetailsId);

                int totalCountOfCartItems = await _cartDetails.GetCartDetailsCountAsync(u => u.CartHeaderId == cartDetailsFromDb.CartHeaderId);

                if (totalCountOfCartItems == 1)
                {
                    var cartHeaderToRemove = await _cartHeader.GetCartHeaderAsync(u => u.CartHeaderId == cartDetailsFromDb.CartHeaderId);

                    await _cartHeader.DeleteCartHeaderAsync(cartHeaderToRemove);
                }

                await _cartDetails.DeleteCartDetailsAsync(cartDetailsFromDb);

                _responseDTO.Result = true;

                return _responseDTO;

            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;

            }

            return _responseDTO;
        }
    }
}
