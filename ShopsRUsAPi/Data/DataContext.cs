using Microsoft.EntityFrameworkCore;
using ShopsRUsAPi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
         : base(options)
        {
        }

     

        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Items> Items { get; set; }
    }
}
