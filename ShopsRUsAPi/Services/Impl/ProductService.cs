using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopsRUsAPi.Data;
using ShopsRUsAPi.Models;
using ShopsRUsAPi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Services.Impl
{
    public class ProductService : IProductService
    {
        private readonly DataContext dataContext;
        private readonly ILogger<ProductService> logger;

        public ProductService(DataContext dataContext, ILogger<ProductService> logger)
        {
            this.dataContext = dataContext;
            this.logger = logger;
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            try
            {

                await dataContext.Products.AddAsync(product);
                var created = await dataContext.SaveChangesAsync();

                return created > 0;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> DeleteProductAsync(long Id)
        {
            try
            {
                var product = await GetProductByIdAsync(Id);
                if (product == null)
                    return false;

                dataContext.Products.Remove(product);
                var deleted = await dataContext.SaveChangesAsync();

                return deleted > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public PagedList<Product> GetProductAsync(PaginationFIlters paginationFIlters)
        {
            try
            {
                var query = dataContext.Products.AsQueryable();
                return new PagedList<Product>(
                    query, paginationFIlters.PageNumber, paginationFIlters.PageSize);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public async Task<Product> GetProductByIdAsync(long id)
        {
            try
            {
                return await dataContext.Products.SingleOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public async Task<List<Product>> GetProductByNameAsync(string name)
        {
            try
            {
                return await dataContext.Products.AsNoTracking().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public async Task<List<long>> InvalidProducts(List<long> id)
        {
            try
            {
                List<long> Invalid_Id = new List<long>();
                foreach(var item in id)
                {
                    var exist = await ProductExist(item);
                    if (!exist)
                    {
                        Invalid_Id.Add(item);
                    }
                    
                }
                return Invalid_Id;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> ProductExist(long id)
        {
            try
            {
                var customer = await dataContext.Products.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
                if (customer == null)
                {
                    return false;
                }

                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> UpdateProduct(long id, Product productToUpdate)
        {
            try
            {
                dataContext.Products.Update(productToUpdate);
                var updated = await dataContext.SaveChangesAsync();
                return updated > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }
    }
}
