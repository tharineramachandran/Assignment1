using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Task6_ANDRE.Models;

namespace Task6_ANDRE.Controllers
{
    public class CartController : Controller
    {

        // GET: Cart/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cart/Create
        [HttpPost]
        public ActionResult Create(ChargeDTO chargeDto)
        {
            try
            {
                StripeConfiguration.ApiKey = "sk_test_51GtwelDUISIWU8NfIfToFjnvtQ3h4paJJ8JtyyXoivVWYrBZJlFqT3hQolgzBJcLe2VeoiugMfgx3LmZuKeNCrkh004UHd1BPs";

                var eventService = new EventService();
                //eventService.Get();

                // `source` is obtained with Stripe.js; see https://stripe.com/docs/payments/accept-a-payment-charges#web-create-token

                var customerOptions = new CustomerCreateOptions
                {
                    Description = chargeDto.CardName,
                    Source = chargeDto.StripeToken,
                    Email = chargeDto.Email,
                    Metadata = new Dictionary<string, string>()
                    {
                        {"Phone Number", chargeDto.Phone }
                    }
                };
                var customerService = new CustomerService();
                Customer customer = customerService.Create(customerOptions);

                var subscriptionOptions = new SubscriptionCreateOptions
                {
                    Customer = customer.Id,
                    Items = new List<SubscriptionItemOptions>
                    {
                        new SubscriptionItemOptions
                        {
                            Price = "price_1GuKOsDUISIWU8NftyytS3Qc",
                        },
                    },
                };

                var service = new SubscriptionService();
                Subscription charge = service.Create(subscriptionOptions);

                var model = new ChargeViewModel();
                model.customerName = Convert.ToString(charge.Schedule);
                model.PriceId = string.Format("{0:#.00}", Convert.ToDecimal(charge.Plan.Amount) / 100);

 
                return View("OrderStatus", model);
            }
            catch
            {
                return View();
            }
        }
    }
}
