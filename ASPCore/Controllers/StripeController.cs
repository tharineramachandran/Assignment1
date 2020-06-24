using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Task6_ANDRE.Controllers
{
 
    public class StripeWebHook : Controller
    {
        // If you are testing your webhook locally with the Stripe CLI you
        // can find the endpoint's secret by running `stripe listen`
        // Otherwise, find your endpoint's secret in your webhook settings in the Developer Dashboard
        const string endpointSecret = "whsec_Go4nAyt5lgiJxFu1mxKzYUJxXjxhiN7P";


        [Route("api/webhooks")]
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            StripeConfiguration.ApiKey = "sk_test_51GtwelDUISIWU8NfIfToFjnvtQ3h4paJJ8JtyyXoivVWYrBZJlFqT3hQolgzBJcLe2VeoiugMfgx3LmZuKeNCrkh004UHd1BPs";

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    Console.WriteLine("PaymentIntent was successful!");
                }
                else if (stripeEvent.Type == Events.PaymentMethodAttached)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                    Console.WriteLine("PaymentMethod was attached to a Customer!");
                }
                // ... handle other event types
                else
                {
                    // Unexpected event type
                    return Json("dfadf");
                }

                return Json("dfadf");
            }
            catch (StripeException)
            {
                return Json("dfadf");
            }
        }
    }
}