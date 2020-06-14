using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Task6.Models;
using Stripe;
namespace Task6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
public ActionResult Charge()
{
	ViewBag.Message = "Learn how to process payments with Stripe";

         var k  = Charge();
	return View(new StripeChargeModel());
}



[HttpPost]
[ValidateAntiForgeryToken]
public async Task<ActionResult> Charge(StripeChargeModel model)
{
	if (!ModelState.IsValid)
	{
		return View(model);
	}



	var chargeId = await ProcessPayment(model);
   // You should do something with the chargeId --> Persist it maybe?
  
	return View("Index");
}



private static async Task<string> ProcessPayment(StripeChargeModel model)
{
   return await Task.Run(() => 
   {
       StripeConfiguration.ApiKey = "pk_test_qOc7BjT6S3JesC6i5RlNevq700ptlnoJmt";

       var myCharge = new Stripe.ChargeCreateOptions()
       {
           // convert the amount of £12.50 to pennies i.e. 1250
           Amount = (int)(model.Amount * 100),
           Currency = "gbp",
           Description = "Description for test charge",
           Source = model.Token
           
       };
   
       var service = new ChargeService();
       var stripeCharge = service.Create(myCharge);
       return stripeCharge.Id;
   });
}

    }
}