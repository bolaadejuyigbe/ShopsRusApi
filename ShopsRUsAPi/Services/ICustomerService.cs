using ShopsRUsAPi.Models;
using ShopsRUsAPi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Services
{
    public  interface ICustomerService
    {
        PagedList <Customer> GetCustomerAsync(PaginationFIlters paginationFIlters);

        Task <Customer> GetCustomerByIdAsync(long CustomerId);
        Task<List<Customer>> GetCustomerByNameAsync(string Name);
        Task<bool> AddCustomerAsync(Customer customer);

        Task<bool> customerExist(long id);


    }
}
