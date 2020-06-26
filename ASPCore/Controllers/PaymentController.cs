using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ASPCore.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// Get Payment intent for refund table at payment 
        public async Task<ActionResult> Get()
        {

            String CustomerID = Request.Form["CustomerID"];
            StripeConfiguration.ApiKey = "sk_test_51GtwelDUISIWU8NfIfToFjnvtQ3h4paJJ8JtyyXoivVWYrBZJlFqT3hQolgzBJcLe2VeoiugMfgx3LmZuKeNCrkh004UHd1BPs";

            var options = new PaymentIntentListOptions
            {
                Customer = CustomerID,
                Limit = 100,
            };
            var service = new PaymentIntentService();
            StripeList<PaymentIntent> paymentIntents = service.List(
              options
            );


            return Json(paymentIntents);
        }


        // refund pass the payment ID from the get and refund it 
        public async Task<ActionResult> Refund()
        {
            String paymentID = Request.Form["paymentID"];
            String amountstr = Request.Form["amount"];
            long amount = (long)Convert.ToDouble(amountstr);


            try
            {
                StripeConfiguration.ApiKey = "sk_test_51GtwelDUISIWU8NfIfToFjnvtQ3h4paJJ8JtyyXoivVWYrBZJlFqT3hQolgzBJcLe2VeoiugMfgx3LmZuKeNCrkh004UHd1BPs";

                var refunds = new RefundService();
                var refundOptions = new RefundCreateOptions
                {
                    PaymentIntent = paymentID,
                    Amount = amount,
                };
                var refund = refunds.Create(refundOptions);

                return Json("Successful");
            }
            catch (Exception ex)
            {

                return Json("Error");


            }



        }



        public ActionResult PausePayment()
        {
            try
            {
                String subscriptionId = Request.Form["subscriptionId"];
                String behaviour = Request.Form["behaviour"];

          // Three values of mark_uncollectible  ,  keep_as_draft   , void


                StripeConfiguration.ApiKey = "sk_test_51GtwelDUISIWU8NfIfToFjnvtQ3h4paJJ8JtyyXoivVWYrBZJlFqT3hQolgzBJcLe2VeoiugMfgx3LmZuKeNCrkh004UHd1BPs";

                var options = new SubscriptionUpdateOptions
                {
                    PauseCollection = new SubscriptionPauseCollectionOptions
                    {
                        Behavior = behaviour,
                    },
                };
                var service = new SubscriptionService();
                service.Update(subscriptionId, options);
                return Json("Successful");
            }
            catch (Exception ex)
            {

                return Json("Error");


            }

        }
    }
}
