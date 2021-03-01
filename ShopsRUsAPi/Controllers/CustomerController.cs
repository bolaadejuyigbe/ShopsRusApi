using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopsRUsAPi.Contracts;
using ShopsRUsAPi.Contracts.V1.Request;
using ShopsRUsAPi.Contracts.V1.Response;
using ShopsRUsAPi.Models;
using ShopsRUsAPi.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopsRUsAPi.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService customerService;
        private readonly ILogger<CustomerController> logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            this.customerService = customerService;
            this.logger = logger;
        }
        /// <summary>
        /// Get All customer Using Pagination
        /// </summary>
        /// <param name="paginationFilter">Pagenumber</param>
        /// <returns>All customeCustomer</returns>
        ///  <response code="200">Retrieves Customers succesfully</response>
        /// <response code="400">Unable to Retrieve Customers from Database due to validation error</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet(ApiRoutes.Customers.GetAll)]
        public async Task<IActionResult> GetAll(PaginationFIlters paginationFilter)
        {
            try
            {
                var model = customerService.GetCustomerAsync(paginationFilter);

                Response.Headers.Add("X-Pagination", model.GetHeader().ToJson());

                var outputModel = new CustomerOutputModel
                {
                    Paging = model.GetHeader(),
                    customerResponses = model.List.Select(x => ToOutputModel(x)).ToList()

                };
                return Ok(outputModel);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
           

        }
        /// <summary>
        /// Retrieves Customer By Id 
        /// </summary>
        /// <param name="customerId">CustomerId</param>
        /// <returns>Returns Customer with a specific Id</returns>
        ///  <response code="200">Retrieves Customers succesfully</response>
        /// <response code="400">Unable to Retrieve Customers with Specified Id from Database due to validation error</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]  
        [Produces("application/json")]
        [HttpGet(ApiRoutes.Customers.Get)]
        public async Task<IActionResult> GetCustomerById([FromRoute] long customerId)
        {
            try
            {
                var cus = await customerService.GetCustomerByIdAsync(customerId);
                if (cus == null)
                    return NotFound();

                return Ok(cus);
            }catch(Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
          
        }


        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>Create Customer Successsfully</returns>
        ///  <response code="201">Customers Created</response>
        /// <response code="400">Unable to Create Customers with Specified Id from Database due to validation error</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost(ApiRoutes.Customers.Create)]
        public async Task<IActionResult> Create([FromBody] CreateCustomer Request)
        {
            try
            {
                if (Request == null)
                {
                    return BadRequest(new ErrorResponse(new ErrorModel { Message = "Request cannot be null" }));
                }

             
                if (Request.isAffiliate == true && Request.isEmployee == true)
                {
                   
                   return BadRequest(new ErrorResponse(new ErrorModel { Message = "You Cannot Be an Affiliate And an Employee Kindly Pick one RoLe" }));
                 
                }
                var model = ToDomainModel(Request);
                var exist = await customerService.customerExist(model.CustomerId);
                if (!exist)
                {
                  var create =  await customerService.AddCustomerAsync(model); if (!create)
                    {
                        return BadRequest(new ErrorResponse(new ErrorModel { Message = "Unable to create Customer" }));
                       
                    }

                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
                    var locationUri = baseUrl + "/" + ApiRoutes.Customers.Get.Replace("{customerId}", model.CustomerId.ToString());
                    var response = ToOutputModel(model);

                    return Created(locationUri, response);
                }
                else
                {
                    return Ok("Customer already exist");
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
           

         

        }
        /// <summary>
        /// Return Customer With a Specific Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ///  <response code="200">Retrieves Customer succesfully</response>
        /// <response code="400">Unable to Retrieve Customers with Specified Name from Database due to validation error</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet(ApiRoutes.Customers.GetbyName)]
        public async Task<IActionResult> GetCustomerByName([FromRoute] string name)
        {
            try
            {
                var cus = await customerService.GetCustomerByNameAsync(name);
                if (cus == null && !cus.Any())
                    return NotFound(cus);
                return Ok(cus);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }



        }

        #region " Mappings "
        private CustomerResponse ToOutputModel(Customer model)
        {
            return new CustomerResponse
            {
               Id = model.CustomerId,
                Address = model.Address,
                Datecreated = model.Datecreated,
                isAffiliate = model.isAffiliate,
                isEmployee = model.isEmployee,
                MobileNum = model.MobileNum,
                Name = model.Name

            };

        }
        private List<CustomerResponse> ToOutputModel(List<Customer> model)
        {
            return model.Select(item => ToOutputModel(item))
                        .ToList();
        }
        private Customer ToDomainModel (CreateCustomer model)
        {
            return new Customer
            {
                CustomerId= model.Id,
                Address = model.Address,
                Datecreated = model.Datecreated,
                isAffiliate = model.isAffiliate,
                isEmployee = model.isEmployee,
                MobileNum = model.MobileNum,
                Name = model.Name
            };
        }
        private CreateCustomer ToInputModel(Customer model)
        {
            return new CreateCustomer
            {
                Id = model.CustomerId,
                Address = model.Address,
                Datecreated = model.Datecreated,
                isAffiliate = model.isAffiliate,
                isEmployee = model.isEmployee,
                MobileNum = model.MobileNum,
                Name = model.Name
            };
        }
        #endregion

    }
}
