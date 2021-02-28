using ShopsRUsAPi.Models;
using ShopsRUsAPi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Services
{
    public interface IProductService
    {
        PagedList<Product> GetProductAsync(PaginationFIlters paginationFIlters);

        Task<Product> GetProductByIdAsync(long id);
        Task<List<Product>> GetProductByNameAsync(string name);

        Task<bool> AddProductAsync(Product product);
        Task<bool> DeleteProductAsync(long Id);

        Task<bool> ProductExist(long id);

        Task<bool> UpdateProduct(long id, Product productToUpdate);

        Task<List<long>> InvalidProducts(List<long> id);
    }
}
