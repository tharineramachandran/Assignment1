using Stripe;
using Stripe.BillingPortal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Task6_ANDRE.Controllers
{
    public class BillingController : Controller
    {
        // GET: Billing/Create
        public ActionResult Test()
        {
            StripeConfiguration.ApiKey = "sk_test_51GtwelDUISIWU8NfIfToFjnvtQ3h4paJJ8JtyyXoivVWYrBZJlFqT3hQolgzBJcLe2VeoiugMfgx3LmZuKeNCrkh004UHd1BPs";
             
            var options = new SubscriptionListOptions
            {
                Limit = 100,
            };
            var service = new SubscriptionService();
            StripeList<Subscription> subscriptions = service.List(
              options
            );


            foreach (var okok in subscriptions) {


                var service1 = new CustomerService();
             var sdf =   service1.Get(okok.CustomerId);

            } 



            return View();
        }

        [HttpPost]
        public ActionResult ManageBilling(FormCollection collection)
        {
            try
            {
                StripeConfiguration.ApiKey = "sk_test_51GtwelDUISIWU8NfIfToFjnvtQ3h4paJJ8JtyyXoivVWYrBZJlFqT3hQolgzBJcLe2VeoiugMfgx3LmZuKeNCrkh004UHd1BPs";

                var options = new SessionCreateOptions
                {
                    Customer = "cus_HWcnIFFtqiH3TB",
                    ReturnUrl = "https://billing.stripe.com/session/{SESSION_SECRET}",
                };
                var service = new SessionService();
                service.Create(options);

            
                return Redirect(options.ReturnUrl);
            }
            catch
            {
                return View();
            }
        }
    }
}
