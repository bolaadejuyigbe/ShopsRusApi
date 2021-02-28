using ShopsRUsAPi.Contracts.V1.Request;
using ShopsRUsAPi.Contracts.V1.Response;
using ShopsRUsAPi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Mapping
{
    public class CustomerMapper
    {
        public CustomerResponse ToOutputModel(Customer model)
        {
            return new CustomerResponse
            {
          
                Address = model.Address,
                Datecreated = model.Datecreated,
                isAffiliate = model.isAffiliate,
                isEmployee = model.isEmployee,
                MobileNum = model.MobileNum,
                Name = model.MobileNum
                
            };

        }
        public List<CustomerResponse> ToOutputModel(List<Customer> model)
        {
            return model.Select(item => ToOutputModel(item))
                        .ToList();
        }

        public Customer ToDomainModel(CreateCustomer inputModel)
        {
            return new Customer
            {
                CustomerId = inputModel.Id,
                Address = inputModel.Address,
                Datecreated = inputModel.Datecreated,
                isAffiliate = inputModel.isAffiliate,
                isEmployee = inputModel.isEmployee,
                MobileNum = inputModel.MobileNum,
                Name = inputModel.Name
               
            };
        }
    }
}
