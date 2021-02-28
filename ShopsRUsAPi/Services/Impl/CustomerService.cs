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
    public class CustomerService : ICustomerService
    {
        private readonly DataContext dataContext;
        private readonly ILogger<CustomerService> logger;

        public CustomerService(DataContext dataContext, ILogger<CustomerService> logger)
        {
            this.dataContext = dataContext;
            this.logger = logger;
        }

        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            try
            {
               
               await dataContext.Customers.AddAsync(customer);
                var created = await dataContext.SaveChangesAsync();
               
                return created >0;

            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> customerExist(long id)
        {
            try
            {
                var customer = await dataContext.Customers.AsNoTracking().SingleOrDefaultAsync(x => x.CustomerId == id);
                if (customer == null)
                {
                    return false;
                }
             
                else
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                return false;
            }
        }

        public PagedList <Customer> GetCustomerAsync(PaginationFIlters paginationFIlters)
        {
            try
            {
                var query =  dataContext.Customers.AsQueryable();
                return new PagedList<Customer>(
                    query, paginationFIlters.PageNumber, paginationFIlters.PageSize);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
           
        }

        public async Task<Customer> GetCustomerByIdAsync(long CustomerId)
        {
            try
            {
                return await dataContext.Customers.SingleOrDefaultAsync(x => x.CustomerId == CustomerId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public async Task<List<Customer>> GetCustomerByNameAsync(string Name)
        {
            try
            {
                return await dataContext.Customers.AsNoTracking().Where(x => x.Name.ToLower().Contains(Name.ToLower())).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }
    }
}
