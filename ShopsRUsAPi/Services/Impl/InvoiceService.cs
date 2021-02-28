using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopsRUsAPi.Contracts.V1.Request;
using ShopsRUsAPi.Data;
using ShopsRUsAPi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Services.Impl
{
    public class InvoiceService : IInvoiceService
    {
        private readonly DataContext dataContext;
        private readonly IDiscountService discountService;
        private readonly IProductService productService;
        private readonly ICustomerService customerService;
        private readonly ILogger<InvoiceService> logger;

        public InvoiceService(DataContext dataContext, IDiscountService discountService, IProductService productService, ICustomerService customerService, ILogger<InvoiceService> logger)
        {
            this.dataContext = dataContext;
            this.discountService = discountService;
            this.productService = productService;
            this.customerService = customerService;
            this.logger = logger;
        }

        public async Task<Invoice> AddInovoiceAsync(CreateInvoice createInvoice)
        {
            try
            {
                bool isvalid = false;
                var customer = await customerService.GetCustomerByIdAsync(createInvoice.customerId);
                //creating an order
                var invoice = await AdditemsToInvoice(createInvoice.createOrders);
                invoice.customer = customer;
                invoice.CustomerId = customer.CustomerId;
             
                    if (customer.isAffiliate)
                    {
                    foreach (var itm in invoice.Items)
                    {
                        if (itm.isGrocery == false)
                        {
                            var discount = "Affiliate_Discount";
                            var getDiscount = await discountService.GetDiscountByTypeAsync(discount);
                            invoice.discountId = getDiscount.DiscountId;
                            invoice.discount = getDiscount;
                            var percent = Convert.ToDecimal(getDiscount.Percentage / 100);
                            var total = percent * (itm.itemAmount * itm.quantity);
                            invoice.totalDiscountAmount = total;
                            invoice.billDiscountPercentage = getDiscount.Percentage;
                            isvalid = true;
                        }
                       
                    }
                    }

                //Calculate Employe discount 
                if (customer.isEmployee)
                {
                    foreach (var itm in invoice.Items)
                    {
                        if (itm.isGrocery == false)
                        {
                            var discount = "Employee_Discount";
                            var getDiscount = await discountService.GetDiscountByTypeAsync(discount);
                            invoice.discountId = getDiscount.DiscountId;
                            invoice.discount = getDiscount;
                            var percent = Convert.ToDecimal(getDiscount.Percentage / 100);
                            var total = percent * (itm.itemAmount * itm.quantity);
                            invoice.billDiscountPercentage = getDiscount.Percentage;
                            invoice.totalDiscountAmount = total;
                            isvalid = true;
                        }

                    }
                }
                    //calculate loyality bonus 
                    if (isvalid == false)
                    {
                    foreach (var itm in invoice.Items)
                    {
                        if (itm.isGrocery == false)
                        {
                            DateTime date = DateTime.Now;
                            var calculateDate = (date - customer.Datecreated).TotalDays.ToString();
                            int datediff = Convert.ToInt32(calculateDate);
                            if (datediff >= 730)
                            {
                                var discount = "Loyality_Discount";

                                var getDiscount = await discountService.GetDiscountByTypeAsync(discount);
                                invoice.discount = getDiscount;
                                invoice.discountId = getDiscount.DiscountId;
                                var percent = Convert.ToDecimal(getDiscount.Percentage / 100);
                                var total = percent * (itm.itemAmount * itm.quantity);
                                invoice.billDiscountPercentage = getDiscount.Percentage;
                                invoice.totalDiscountAmount = total;
                                isvalid = true;
                            }
                        }
                    }

                    }

                
                //Calculate Affiliate discount 


                //Calculate $100 Discount
                var dicountedAmount = 100;
                if(invoice.totalAmount >= 100)
                {
                    var rem = (invoice.totalAmount % dicountedAmount);
                    var diff = invoice.totalAmount - rem;
                    var div = diff / dicountedAmount;
                    var totalDisc = div * 5;
              
                    if (invoice.discountId == 0)
                    {
                        var discount = "100_Dollar_discount";

                        var getDiscount = await discountService.GetDiscountByTypeAsync(discount);
                        invoice.discount = getDiscount;
                        invoice.billDiscountPercentage = 5;

                    }

                    invoice.totalDiscountAmount = invoice.totalDiscountAmount + totalDisc;
                    invoice.updateTotalwithdiscount();
                    invoice.DateCreated = DateTime.Now;

                }
                else
                {
                    var discnt = "Zero_Discount";
                    if (invoice.discountId == 0)
                    {
                        var getDiscount = await discountService.GetDiscountByTypeAsync(discnt);
                        invoice.discount = getDiscount;
                        invoice.discountId = getDiscount.DiscountId;
                        invoice.billDiscountPercentage = getDiscount.Percentage;
                    }
                 
                   
                    invoice.DateCreated = DateTime.Now;
                }

             
                await dataContext.Invoices.AddAsync(invoice);
                 await dataContext.SaveChangesAsync();
                return invoice;


            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        private async Task<Invoice> AdditemsToInvoice(List<CreateOrder> createOrders)
        {
            try
            {
                Invoice invoice = new Invoice();
                List<Items> items = new List<Items>();
                foreach(var item in createOrders)
                {
                    var product = await productService.GetProductByIdAsync(item.productId);
                    if (product != null)
                    {
                        var data = new Items();
                        data.quantity = item.quantity;
                        data.itemAmount = product.Amount;
                        data.productId = product.Id;
                        data.isGrocery = product.IsGroccery;
                        data.products = product;
                        invoice.totalproductAmount += (data.itemAmount * data.quantity);
                        items.Add(data);

                    }
                   
                }
                invoice.Items = items;
                invoice.totalAmount = invoice.totalproductAmount;
                return invoice;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public async Task<decimal> GetTotalAmountDetailsbyInvoiceNumberAsync(long invoiceNumber)
        {
           try
            {
                return await dataContext.Invoices.Where(x => x.Id == invoiceNumber).Select(e => e.totalAmount).SingleOrDefaultAsync();
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> InvoiceExist(long id)
        {
            try
            {
                var discount = await dataContext.Invoices.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
                if (discount == null)
                {
                    return false;
                }

                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message + ":" + ex.StackTrace);
                throw;
            }
        }
    }
}
