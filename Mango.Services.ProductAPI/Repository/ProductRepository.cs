using System;
using System.Linq.Expressions;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Model;
using Mango.Services.ProductAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repository
{
	public class ProductRepository : IProductRepository
	{
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
		{
            _appDbContext = appDbContext;
        }

        public async Task CreateProductAsync(Product entity)
        {

            await _appDbContext.Products.AddAsync(entity);

           await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllProductAsync(Expression<Func<Product, bool>> filter = null)
        {
            IQueryable<Product> query = _appDbContext.Products;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Product?> GetProductAsync(Expression<Func<Product, bool>> filter = null)
        {
            IQueryable<Product> query = _appDbContext.Products;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Product?> RemoveProductAsync(Product entity)
        {
            _appDbContext.Products.Remove(entity);

            await _appDbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Product?> UpdateProductAsync(int productId,Product entity)
        {

            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity is empty");
            }
            var model = await _appDbContext.Products.FirstOrDefaultAsync(u => u.ProductId == productId);

            if (model == null)
            {
                return null;
            }

            model.CategoryName = entity.CategoryName;
            model.Description = entity.Description;
            model.ImageUrl = entity.ImageUrl;
            model.Name = entity.Name;
            model.Price = entity.Price;
            //model.ProductId = entity.ProductId;

            //_appDbContext.Entry(entity).State = EntityState.Modified;

            await _appDbContext.SaveChangesAsync();

            return entity;
        }
    }
}

