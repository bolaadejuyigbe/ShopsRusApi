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
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9 ]*$");
            RuleFor(x => x.MobileNum)
               .NotEmpty()
               .Matches("^[0-9]{3}-[0-9]{4}-[0-9]{4}$");

        }
    }
}

