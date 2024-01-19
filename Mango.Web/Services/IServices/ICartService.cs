﻿using System;
using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
	public interface ICartService
	{
        Task<ResponseDTO?> GetCartByUserIdAsync(string userId);

        Task<ResponseDTO?> UpsertCartAsync(CartDTO cartDTO);

        Task<ResponseDTO?> RemoveFromCartAsync(int cartDetailsId);

        Task<ResponseDTO?> ApplyCouponAsync(CartDTO cartDTO);

        Task<ResponseDTO?> EmailCart(CartDTO cartDTO);
    }
}

