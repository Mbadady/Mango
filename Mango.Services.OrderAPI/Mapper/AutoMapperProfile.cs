using System;
using AutoMapper;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.DTO;

namespace Mango.Services.OrderAPI.Mapper
{
	public class AutoMapperProfile : Profile
	{
        public AutoMapperProfile()
        {
            CreateMap<OrderDetails, OrderDetailsDTO>().ReverseMap();
            CreateMap<OrderHeader, OrderHeaderDTO>().ReverseMap();

            CreateMap<OrderHeaderDTO, CartHeaderDTO>().ForMember(dest => dest.CartTotal, u => u.MapFrom(src => src.OrderTotal)).ReverseMap();

            CreateMap<CartDetailsDTO, OrderDetailsDTO>()
                .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.ProductDTO.Name))
                .ForMember(dest => dest.Price, u => u.MapFrom(src => src.ProductDTO.Price));

            CreateMap<OrderDetailsDTO, CartDetailsDTO>();
        }
    }
}

