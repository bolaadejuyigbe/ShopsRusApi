using ShopsRUsAPi.Models;
using ShopsRUsAPi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Services
{
    public  interface IDiscountService
    {
        PagedList<Discount> GetDiscountAsync(PaginationFIlters paginationFIlters);

        Task<Discount> GetDiscountByTypeAsync(string type);
        Task<Discount> GetDiscountByIdAsync(long id);

        Task<bool> AddDiscountAsync(Discount discount);
        Task<bool> DeleteDiscountAsync(long Id);

        Task<bool> DiscountExist(long id);
    }
}
