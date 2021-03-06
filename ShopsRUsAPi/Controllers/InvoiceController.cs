﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopsRUsAPi.Contracts;
using ShopsRUsAPi.Contracts.V1.Request;
using ShopsRUsAPi.Services;

namespace ShopsRUsAPi.Controllers
{
   
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService invoiceService;
        private readonly ICustomerService customerService;
        private readonly IDiscountService discountService;
        private readonly IProductService productService;
        private readonly ILogger<InvoiceController> logger;

        public InvoiceController(IInvoiceService invoiceService, ICustomerService customerService, IDiscountService discountService, IProductService productService, ILogger<InvoiceController> logger)
        {
            this.invoiceService = invoiceService;
            this.customerService = customerService;
            this.discountService = discountService;
            this.productService = productService;
            this.logger = logger;

        }


        /// <summary>
        /// Get Amount By invoice_Id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns>Amount for a particular Invoice</returns>
        /// <response code="200">Returns price for Invoice succesfully</response>
        /// <response code="400">Unable to Return Invoice Price due to validation error</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet(ApiRoutes.Invoice.Get)]
        public async Task<ActionResult<decimal>> GetTotalAmountByInvoiceId([FromRoute] long invoiceId)
        {
            try
            {
                var exist = await invoiceService.InvoiceExist(invoiceId);
                if (!exist)
                {
                    return NotFound();
                }
                

                return await invoiceService.GetTotalAmountDetailsbyInvoiceNumberAsync(invoiceId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }

        }


        // POST: api/Invoice
        /// <summary>
        /// Create Invoice
        /// </summary>
        /// <param name="Request"></param>
        /// <returns>Inoice Created Successfully</returns>
        /// <response code="201">Create Invoice succesfully</response>
        /// <response code="400">Unable to Create Invoice Price due to validation error</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost(ApiRoutes.Invoice.Create)]
        public async Task<IActionResult> Create([FromBody] CreateInvoice Request)
        {
            try
            {
                List<long> listOfProductIds = new List<long>();
             
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        error = "Your Model state is not valid"
                    });


                }
                var exist = await customerService.customerExist(Request.customerId);
                if (!exist)
                {
                    return BadRequest(new
                    {
                        error = "Not a registered Customer"
                    });

                }
                Request.createOrders.ForEach(x => listOfProductIds.Add(x.productId));
                var invalidProduct = await productService.InvalidProducts(listOfProductIds);
                if (invalidProduct.Any())
                {
                    return NotFound(new
                    {
                        error = "product(s) ids are Invalid:" + string.Join(",", invalidProduct)
                    });
                }
                var created = await invoiceService.AddInovoiceAsync(Request);
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
                var locationUri = baseUrl + "/" + ApiRoutes.Invoice.Get.Replace("{invoiceId}", Request.Id.ToString());
                return Created(locationUri, created);


            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }

        }
 
    }
}
