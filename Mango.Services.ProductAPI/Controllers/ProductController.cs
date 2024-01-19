using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mango.Services.ProductAPI.Model;
using Mango.Services.ProductAPI.Model.DTO;
using Mango.Services.ProductAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        
        public ProductController(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> GetAllProduct()
        {
            try
            {

                List<Product> products = await _productRepository.GetAllProductAsync();

                ResponseDTO responseDTO = new ResponseDTO();

                if (products.Count == 0)
                {
                    responseDTO.Message = "Product is empty";

                    return NotFound(responseDTO);
                }

                responseDTO.Result = _mapper.Map<List<ProductDTO>>(products);

                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                ResponseDTO responseDTO = new ResponseDTO();

                responseDTO.Message = ex.Message;

                return StatusCode(500, responseDTO);
            }
        }

        [HttpGet]
        [Route("{productId:int}", Name = "GetProduct")]
        public async Task<ActionResult<ResponseDTO>> GetProduct([FromRoute]int productId)
        {
            try
            {
                

                var product = await _productRepository.GetProductAsync(u => u.ProductId == productId);

                ResponseDTO responseDTO = new ResponseDTO();

                if(product == null)
                {
                    responseDTO.IsSuccess = false;
                    responseDTO.Message = $"Product with product Id {productId} not found";
                    return NotFound(responseDTO);
                }

                responseDTO.Result = _mapper.Map<ProductDTO>(product);

                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                ResponseDTO responseDTO = new ResponseDTO();

                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;

                return StatusCode(500, responseDTO);
            }

        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDTO>> CreateProduct([FromBody] CreateProductDTO createProductDTO)
        {

            try
            {
                if (createProductDTO == null)
                {
                    return BadRequest(createProductDTO);
                }

                if (await _productRepository.GetProductAsync(u => u.Name.ToLower() == createProductDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessage", "Product Name already exists");
                }

                var model = _mapper.Map<Product>(createProductDTO);
                var responseDTO = new ResponseDTO();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _productRepository.CreateProductAsync(model);

                responseDTO.Result = _mapper.Map<ProductDTO>(model);

                return CreatedAtRoute("GetProduct", new { productId = model.ProductId }, responseDTO);


            }
            catch (Exception ex)
            {
                ResponseDTO responseDTO = new ResponseDTO();

                responseDTO.IsSuccess = false;
                responseDTO.Message = ex.Message;

                return StatusCode(500, responseDTO);
            }

        }

        [HttpPut]
        [Route("{productId:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDTO>> UpdateProduct([FromRoute] int productId,[FromBody] UpdateProductDTO updateProductDTO)
        {

            try
            {

                if(await _productRepository.GetProductAsync(u => u.ProductId == productId) == null)
                {
                    return NotFound();
                }

                var model = _mapper.Map<Product>(updateProductDTO);
                

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                model = await _productRepository.UpdateProductAsync(productId, model);

                var responseDTO = new ResponseDTO
                {
                    Result = _mapper.Map<ProductDTO>(model)
                };
                

                return Ok(responseDTO);


            }
            catch (Exception ex)
            {
                ResponseDTO responseDTO = new ResponseDTO()
                {
                    IsSuccess = false,
                    Message = ex.Message
            };
                return StatusCode(500, responseDTO);
            }

        }

        [HttpDelete]
        [Route("{productId:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDTO>> DeleteProduct([FromRoute]int productId)
        {
            try
            {
                if(productId == 0)
                {
                    return BadRequest();
                }

                var product = await _productRepository.GetProductAsync(u => u.ProductId == productId);

                if(product == null)
                {
                    return NotFound();
                }

                product = await _productRepository.RemoveProductAsync(product);

                var responseDTO = new ResponseDTO()
                {
                    Result = _mapper.Map<ProductDTO>(product)
                };

                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                ResponseDTO responseDTO = new ResponseDTO()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
                return StatusCode(500, responseDTO);
            }

        }
        }


    }

