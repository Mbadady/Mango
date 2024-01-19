using System;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.DTO;

namespace Mango.Services.OrderAPI.Repository.IRepository
{
	public interface IOrderRepository
	{
		Task<OrderHeader> CreateOrderAsync(OrderHeader orderHeader);

	}
}

