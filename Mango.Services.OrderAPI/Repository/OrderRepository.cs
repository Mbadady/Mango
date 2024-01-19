using System;
using Mango.Services.OrderAPI.Data;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.DTO;
using Mango.Services.OrderAPI.Repository.IRepository;

namespace Mango.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;

        public OrderRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<OrderHeader> CreateOrderAsync(OrderHeader orderHeader)
        {
            await _db.OrderHeaders.AddAsync(orderHeader);
            await _db.SaveChangesAsync();

            return orderHeader;
        }
    }
}

