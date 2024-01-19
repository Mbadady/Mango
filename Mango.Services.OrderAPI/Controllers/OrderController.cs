using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.DTO;
using Mango.Services.OrderAPI.Repository.IRepository;
using Mango.Services.OrderAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.OrderAPI.Controllers
{
    [Route("api/Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IOrderRepository _orderRepository;
        protected ResponseDTO _response;

        public OrderController(IMapper mapper, IProductService productService, IOrderRepository orderRepository)
        {
            _mapper = mapper;
            _productService = productService;
            _orderRepository = orderRepository;
            _response = new();
        }

        [Authorize]
        [HttpPost("CreateOrder")]
        public async Task<ResponseDTO> CreateOrder([FromBody] CartDTO cartDTO)
        {
            try
            {
                OrderHeaderDTO orderHeaderDTO = _mapper.Map<OrderHeaderDTO>(cartDTO.CartHeaderDTO);
                orderHeaderDTO.OrderTime = DateTime.Now;
                orderHeaderDTO.OrderDetailsDTO = _mapper.Map<IEnumerable<OrderDetailsDTO>>(cartDTO.CartDetailsDTO);


               OrderHeader orderCreated = await _orderRepository.CreateOrderAsync(_mapper.Map<OrderHeader>(orderHeaderDTO));

                orderHeaderDTO.OrderHeaderId = orderCreated.OrderHeaderId;
                _response.Result = orderHeaderDTO;

                //return _response;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
