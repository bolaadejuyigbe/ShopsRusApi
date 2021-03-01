using FluentValidation;
using ShopsRUsAPi.Contracts.V1.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Validators
{
    public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomer>
    {
        public  CreateCustomerRequestValidator()
        {
           
           

        }
    }
}

