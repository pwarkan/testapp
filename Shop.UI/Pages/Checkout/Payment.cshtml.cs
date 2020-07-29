using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Shop.Application.Cart;
using Shop.Application.Orders;
using Shop.Database;
using Shop.Domain.Infrastructure;
using Stripe;
using GetOrderCart = Shop.Application.Cart.GetOrder;

namespace Shop.UI.Pages.Checkout
{
    public class PaymentModel : PageModel
    {
        public PaymentModel (IConfiguration config)
        {
            PublicKey = config["Stripe:PublicKey"].ToString();
        }

        public string PublicKey { get; }

        public IActionResult OnGet([FromServices] GetCustomerInformation getCustomerInformation)
        {
            var information = getCustomerInformation.Do();

            if (information == null)
            {
                return RedirectToPage("/Checkout/CustomerInformation");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(
            string stripeEmail, 
            string stripeToken,
            [FromServices] GetOrderCart getOrder,
            [FromServices] CreateOrder createOrder,
            [FromServices] ISessionManager sessionManager)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var CartOrder = getOrder.Do();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = CartOrder.GetTotalCharge(),
                Description = "Shop Purchase",
                Currency = "usd",
                Customer = customer.Id
            });

            var sessionId = HttpContext.Session.Id;


            await createOrder.Do(new CreateOrder.Request
            {
                StripeReference = charge.Id,
                SessionId = sessionId,

                FirstName = CartOrder.CustomerInformation.FirstName,
                LastName = CartOrder.CustomerInformation.LastName,
                Email = CartOrder.CustomerInformation.Email,
                PhoneNumber = CartOrder.CustomerInformation.PhoneNumber,
                Address1 = CartOrder.CustomerInformation.Address1,
                Address2 = CartOrder.CustomerInformation.Address2,
                City = CartOrder.CustomerInformation.City,
                PostCode = CartOrder.CustomerInformation.PostCode,

                Stocks = CartOrder.Products.Select(x => new CreateOrder.Stock
                {
                    StockId = x.StockId,
                    Qty = x.Qty,
                }).ToList()
            });

            sessionManager.ClearCart();

            return RedirectToPage("/Index");
        }
    }
}
