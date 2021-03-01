using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopsRUsAPi.Contracts;
using ShopsRUsAPi.Contracts.V1.Request;
using ShopsRUsAPi.Contracts.V1.Response;
using ShopsRUsAPi.Models;
using ShopsRUsAPi.Services;

namespace ShopsRUsAPi.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IDiscountService discountService;
        private readonly ILogger<DiscountController> logger;

        public DiscountController(IDiscountService discountService, ILogger<DiscountController> logger)
        {
            this.discountService = discountService;
            this.logger = logger;
        }


        /// <summary>
        /// Get list of Discount
        /// </summary>
        /// <param name="paginationFilter"></param>
        /// <returns>All Discount</returns>
        ///  <response code="200">Retrieves Discounts succesfully</response>
        /// <response code="400">Unable to Retrieve Discount from Database due to validation error</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet(ApiRoutes.Discount.GetAll)]
        public async Task<IActionResult> GetAll(PaginationFIlters paginationFilter)
        {
            try
            {
                var model = discountService.GetDiscountAsync(paginationFilter);

                Response.Headers.Add("X-Pagination", model.GetHeader().ToJson());

                var outputModel = new DiscountOutputModel
                {
                    Paging = model.GetHeader(),
                    discountResponses = model.List.Select(x => ToOutputModel(x)).ToList()

                };
                return Ok(outputModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }


        }
        /// <summary>
        /// Get discount by Type
        /// </summary>
        /// <param name="type"></param>
  
        /// <returns>Discount with A specific Type </returns>
        ///  <response code="200">Retrieves Discounts with Specified Type succesfully</response>
        /// <response code="400">Unable to Retrieve Discount from Database due to validation error</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet(ApiRoutes.Discount.Get)]
        public async Task<IActionResult> GetDiscountPercentageByType([FromRoute] string type)
        {
            try
            {
                var discount = await discountService.GetDiscountByTypeAsync(type);
                if (discount == null)
                    return NotFound();
                return Ok(discount);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }



        }
        /// <summary>
        /// Delete Discount
        /// </summary>
        /// <param name="discountId"></param>
        /// <returns>No content</returns>
        ///  <response code="204">Delete Discount with Specified Id succesfully</response>
        /// <response code="400">Unable to Delete Discount from Database due to validation error</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete(ApiRoutes.Discount.Delete)]
        public async Task<IActionResult> Delete([FromRoute] long  discountId)
        {
            try
            {
                var deleted = await discountService.DeleteDiscountAsync(discountId);
                if (deleted)
                    return NoContent();

                return NotFound();
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
           
        }

        /// <summary>
        /// Create Discount 
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>discount Created </returns>
        /// <response code="201">Create Discounts succesfully</response>
        /// <response code="400">Unable to Create Discount from Database due to validation error</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost(ApiRoutes.Discount.Create)]
        public async Task<IActionResult> Create([FromBody] CreateDiscount Request)
        {
            try
            {
                if (Request == null)
                    return BadRequest(
                        new
                        {
                            error = "Your Request Cannot be Null"
                        });

                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        error = "Your Model state is not valid"
                    });
                }

                var model = ToDomainModel(Request);
                var exist = await discountService.DiscountExist(model.DiscountId);
                if (!exist)
                {
                   var create = await discountService.AddDiscountAsync(model);
                    if (!create)
                    {
                        return BadRequest(new { error = "Unable to create Discount" });
                    }

                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
                    var locationUri = baseUrl + "/" + ApiRoutes.Discount.Get.Replace("{discountId}", model.DiscountId.ToString());
                    var response = ToOutputModel(model);

                    return Created(locationUri, response);
                }
                else
                {
                    return Ok("Discount Type already exist");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }

        }
        #region " Mappings "
        private DiscountResponse ToOutputModel(Discount model)
        {
            return new DiscountResponse
            {

               Type = model.Type,
               Name = model.Name,
               Percentage = model.Percentage

            };

        }
        private  List<DiscountResponse> ToOutputModel(List<Discount> model)
        {
            return  model.Select(item => ToOutputModel(item))
                        .ToList();
        }
        private Discount ToDomainModel(CreateDiscount model)
        {
            return new Discount
            {
                DiscountId = model.Id,
                Type = model.Type,
                Name = model.Name,
                Percentage = model.Percentage
                
            };
        }
        private CreateDiscount ToInputModel(Discount model)
        {
            return new CreateDiscount
            {
                Id = model.DiscountId,
                Type = model.Type,
                Name = model.Name,
                Percentage = model.Percentage
            };
        }
        #endregion
    }


}