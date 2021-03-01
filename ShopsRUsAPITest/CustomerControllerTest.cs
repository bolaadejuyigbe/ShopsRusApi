using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShopsRUsAPi.Contracts;
using ShopsRUsAPi.Contracts.V1.Request;
using ShopsRUsAPi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShopsRUsAPITest
{
    public class CustomerControllerTest : IntegrationTest
    {
       
 
        [Fact]
        public async Task Get_ReturnsPost_WhenPostExistsInTheDatabase()


        {
            // Arrange
           
            var createdPost = await CreatePostAsync(new CreateCustomer
            {
                Id =1,
                Name = "Bola",
                Address = "Lagos 3",
                Datecreated = DateTime.Now,
                isAffiliate = false,
                isEmployee = true,
                MobileNum = "0816098271",
            

            });

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Customers.Get.Replace("{customerId}", createdPost.Id.ToString()));
             
            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedPost = await response.Content.ReadAsAsync<Customer>(
                );


            returnedPost.CustomerId.Should().Be(createdPost.Id);
            returnedPost.Name.Should().Be("Bola");



        }
        

    }
}