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
    public class DiscountService : IDiscountService
    {
        private readonly ILogger<DiscountService> logger;
        private readonly DataContext dataContext;

        public DiscountService(ILogger<DiscountService> logger, DataContext dataContext)
        {
            this.logger = logger;
            this.dataContext = dataContext;
        }

        public async Task<bool> AddDiscountAsync(Discount discount)
        {
            try
            {
                await dataContext.Discounts.AddAsync(discount);
                var created = await dataContext.SaveChangesAsync();

                return created > 0;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> DeleteDiscountAsync(long Id)
        {
            try
            {
                var discount = await GetDiscountByIdAsync(Id);
                if (discount == null)
                    return false;

                dataContext.Discounts.Remove(discount);
                var deleted = await dataContext.SaveChangesAsync();

                return deleted > 0;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> DiscountExist(long id)
        {
            try
            {
                var discount = await dataContext.Discounts.AsNoTracking().SingleOrDefaultAsync(x => x.DiscountId == id);
                if (discount == null)
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

        public PagedList<Discount> GetDiscountAsync(PaginationFIlters paginationFIlters)
        {
            try
            {
                var query = dataContext.Discounts.AsQueryable();
                return new PagedList<Discount>(
                    query, paginationFIlters.PageNumber, paginationFIlters.PageSize);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public async Task<Discount> GetDiscountByIdAsync(long id)
        {
            try
            {
                return await dataContext.Discounts.AsNoTracking().SingleOrDefaultAsync(x => x.DiscountId == id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public async Task<Discount> GetDiscountByTypeAsync(string type)
        {
            try
            {
                var disount = await dataContext.Discounts.SingleOrDefaultAsync(x => x.Type.ToLower()==(type.ToLower()));
              
                return disount;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }
    }
}
