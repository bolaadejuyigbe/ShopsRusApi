using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Contracts
{
    public class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "V1";
        public const string Base = Root + "/" + Version;
        public static class Customers
        {
            public const string GetAll = Base + "/Customers";
            public const string Get = Base + "/Customers/{customerId}";
            public const string GetbyName = Base + "/SearchCustomer/{name}";
            public const string Create = Base + "/Customer";
  
        }
        public static class Discount
        {
            public const string GetAll = Base + "/Discounts";
            public const string Get = Base + "/Discount/{type}";
            public const string Create = Base + "/Discount";
            public const string Delete = Base + "/Discounts/{discountId}";

        }

        public static class Invoice
        {
          
            public const string Get = Base + "/Invoice/{invoiceId}";
            public const string Create = Base + "/Invoice";
          

        }
        public static class Products
        {
            public const string GetAll = Base + "/Products";
            public const string Get = Base + "/Products/{productId}";
            public const string GetbyName = Base + "/SearchProducts/{name}";
            public const string Create = Base + "/Products";
            public const string Delete = Base + "/Products/{productId}";
            public const string Update = Base + "/Products/{productId}";

        }
    }
}
