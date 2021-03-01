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
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly ILogger<ProductController> logger;


        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }

        // GET: api/Product
              /// <summary>
              /// 
              /// </summary>
              /// <param name="paginationFilter"></param>
              /// <returns></returns>
        [HttpGet(ApiRoutes.Products.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery]PaginationFIlters paginationFilter)
        {
            try
            {
                var model = productService.GetProductAsync(paginationFilter);

                Response.Headers.Add("X-Pagination", model.GetHeader().ToJson());

                var outputModel = new ProductOutputModel
                {
                    Paging = model.GetHeader(),
                    ProductResponses = model.List.Select(x => ToOutputModel(x)).ToList()

                };
                return Ok(outputModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        // GET: api/Product/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Products.Get)]
        public async Task<IActionResult> GetProductById([FromRoute] long productId)
        {
            try
            {
                var prod = await productService.GetProductByIdAsync(productId);
                if (prod == null)
                    return NotFound();

                return Ok(prod);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Products.GetbyName)]
        public async Task<IActionResult> GetProductByName([FromRoute] string name)
        {
            try
            {
                var products = await productService.GetProductByNameAsync(name);
                if (products == null && !products.Any())
                    return NotFound();
                return Ok(products);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }



        }
        // POST: api/Product
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        [HttpPost(ApiRoutes.Products.Create)]
        public async Task<IActionResult> Create([FromBody] CreateProducts Request)
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
                var exist = await productService.ProductExist(model.Id);
                if (!exist)
                {
                   var create =  await productService.AddProductAsync(model);
                    if (!create)
                    {
                        return BadRequest(new { error = "Unable to create Product" });
                    }

                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
                    var locationUri = baseUrl + "/" + ApiRoutes.Products.Get.Replace("{Id}", model.Id.ToString());
                    var response = ToOutputModel(model);

                    return Created(locationUri, response);
                }
                else
                {
                    return Ok("Product already exist");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }

        }
        // PUT: api/Product/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut(ApiRoutes.Products.Update)]
        public async Task<IActionResult> Update([FromRoute] long productId, [FromBody] Product request)
        {
            try
            {
                if (request.Id != productId)
                {
                    return BadRequest(new
                    {
                        error = "You are trying to update a product with different Id"
                    });
                }
                var prod = await productService.GetProductByIdAsync(productId);
              
                if (prod != null)
                {
                    var updated = await productService.UpdateProduct(productId, request);
                    if (updated)
                    {
                        return Ok(prod);
                    }
                    else
                    {
                        return NotFound();
                    }          
                }
                return NotFound();
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
           
           
          
           
        }

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete(ApiRoutes.Products.Delete)]
        public async Task<IActionResult> Delete([FromRoute] long productId)
        {
            try
            {
                var deleted = await productService.DeleteProductAsync(productId);
                if (deleted)
                    return NoContent();

                return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }

        }
        #region " Mappings "
        private ProductResponse ToOutputModel(Product model)
        {
            return new ProductResponse
            {

           
                Amount = model.Amount,
                Name = model.Name,
                IsGroccery = model.IsGroccery

            };

        }
        private List<ProductResponse> ToOutputModel(List<Product> model)
        {
            return model.Select(item => ToOutputModel(item))
                        .ToList();
        }
        private Product ToDomainModel(CreateProducts model)
        {
            return new Product
            {
                Id = model.Id,
                Amount = model.Amount,
                Name = model.Name,
                IsGroccery = model.IsGroccery

            };
        }

     

        private CreateProducts ToInputModel(Product model)
        {
            return new CreateProducts
            {
                Id = model.Id,
                Amount = model.Amount,
                Name = model.Name,
                IsGroccery = model.IsGroccery
            };
        }
        #endregion
    }
}
