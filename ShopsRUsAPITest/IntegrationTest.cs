using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ShopsRUsAPi;
using ShopsRUsAPi.Contracts;
using ShopsRUsAPi.Contracts.V1.Request;
using ShopsRUsAPi.Contracts.V1.Response;
using ShopsRUsAPi.Data;
using System;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ShopsRUsAPi.Models;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;
using ShopsRUsAPi.Utils;
using Newtonsoft.Json;

namespace ShopsRUsAPITest
{
   public class IntegrationTest : IDisposable
    {

        private readonly IServiceProvider _serviceProvider;
        protected readonly HttpClient TestClient;

        protected IntegrationTest()
        {
            var connection = new SqliteConnection("DataSource=C:\\Sqlite\\TestDb");
         
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DataContext));
                          //services.RemoveAll(typeof(DataContext));

                          services.AddDbContext<DataContext>(options => { options.UseSqlite(connection); });
                    });
                });
           
            _serviceProvider = appFactory.Services;
            TestClient = appFactory.CreateClient();


        }
   
        protected async Task<CustomerResponse> CreatePostAsync(CreateCustomer request)
        {
           
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Customers.Create, request);
      
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            
            return (await response.Content.ReadAsAsync<CustomerResponse>());

        }
     
        public void Dispose()
        {

            using var serviceScope = _serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DataContext>();
            context.Database.EnsureDeleted();
        }

       
  
    }


}



