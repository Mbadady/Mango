using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTOs;
using Mango.Services.CouponAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        private ResponseDTO _response;

        public CouponAPIController(ICouponRepository couponRepository, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDTO>> Get()
        {

            try
            {

                var objList = await _couponRepository.GetAllAsync();

                _response.Result = _mapper.Map<List<CouponDTO>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return Ok(_response);
            
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ResponseDTO>> GetById([FromRoute]int id)
        {
            try
            {

                var couponDomainModel = await _couponRepository.GetAsync(x => x.CouponId == id);

                if (couponDomainModel != null)
                {
                    _response.Result = _mapper.Map<CouponDTO>(couponDomainModel);
                }

                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return Ok(_response);
            
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public async Task<ActionResult<ResponseDTO>> GetByCode([FromRoute] string code)
        {
            try
            {

                var couponDomainModel = await _couponRepository.GetAsync(x => x.CouponCode.ToLower() == code.ToLower());

                if (couponDomainModel != null)
                {
                    _response.Result = _mapper.Map<CouponDTO>(couponDomainModel);
                }

                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return Ok(_response);

        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDTO>> Create([FromBody]CouponDTO couponDTO)
        {
            try
            {

                var domainModel = _mapper.Map<Coupon>(couponDTO);

                await _couponRepository.CreateAsync(domainModel);

                _response.Result = _mapper.Map<CouponDTO>(domainModel);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return Ok(_response);
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDTO>> Update([FromBody] CouponDTO couponDTO, [FromRoute]int id)
        {
            try
            {

                var domainModel = _mapper.Map<Coupon>(couponDTO);

                domainModel = await _couponRepository.UpdateAsync(id, domainModel);

                if(domainModel == null)
                {
                    return NotFound();
                }

                _response.Result = _mapper.Map<CouponDTO>(domainModel);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return Ok(_response);
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDTO>> Delete([FromRoute] int id)
        {
            try
            {
                var domainModel = await _couponRepository.DeleteAsync(id);

                if(domainModel == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return Ok(_response);
        }
    }
}
