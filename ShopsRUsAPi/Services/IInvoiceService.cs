using ShopsRUsAPi.Contracts.V1.Request;
using ShopsRUsAPi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRUsAPi.Services
{
    public interface IInvoiceService
    {
        Task<Invoice> AddInovoiceAsync(CreateInvoice createInvoice);

        Task<bool> InvoiceExist(long id);

        Task<decimal> GetTotalAmountDetailsbyInvoiceNumberAsync(long invoiceNumber);
    }
}
