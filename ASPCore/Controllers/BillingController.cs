using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.BillingPortal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Task6_ANDRE.Models;

namespace Task6_ANDRE.Controllers
{
    public class BillingController : Controller
    {
        // GET: Billing/Create
        public ActionResult ManageBilling()
        {
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
