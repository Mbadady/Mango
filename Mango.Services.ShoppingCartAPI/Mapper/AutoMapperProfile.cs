using System;
using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.DTO;

namespace Mango.Services.ShoppingCartAPI.Mapper
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<CartDetails, CartDetailsDTO>().ReverseMap();
			CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();
		}
	}
}

