using FireSharp;
using FireSharp.Config;
using FireSharp.Extensions;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "WlzsaHKFjCMdBFa1lEvOr36PR2oFvEZDEPFPCJkG",
            BasePath = "https://refundcsc.firebaseio.com/"
        };

        FirebaseClient client;

        string custName, amt;

        [Route("api/webhooks")]
        [HttpPost]
        public async Task<IActionResult> Index(Models.ChargeViewModel chargeViewModel)
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
                    var asdf = stripeEvent.Data.Object;
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;


                    var customerJson = asdf.ToJson();

                    JObject jsonName = JObject.Parse(customerJson);
                    string custName = Convert.ToString(jsonName["customer"]);
                    string amt = Convert.ToString(jsonName["amount"]);
                    // call firebase here with cumID and price
                    chargeViewModel.customerName = custName;
                    chargeViewModel.PriceId = amt;

                    AddSubscription(chargeViewModel);
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
        private void AddSubscription(Models.ChargeViewModel chargeViewModel)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = chargeViewModel;
            //PushResponse response = client.Push("Subscription/", data);
            SetResponse setResponseName = client.Set("Subscription/" + data.customerName, data);
        }
    }
}