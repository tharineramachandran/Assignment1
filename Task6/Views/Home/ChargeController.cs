    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
 using Task6.Models;
 using System.Web.Mvc;
using Stripe;
using Microsoft.Ajax.Utilities;
using System.Net.Http;
using System.Net;

namespace Task6.Views.Home
{
    public class ChargeController : Controller
    {
        // GET: Charge
        public ActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Charge(StripeChargeModel model)
        { //    4242424242424242
            string errormessage = "";
            bool isvalidemail = false;
            bool isvalidamount = false;
            HttpResponseMessage responseMessage = new HttpResponseMessage();

            try
            {
                var addr = new System.Net.Mail.MailAddress(model.Email);
                bool emailvalid = (addr.Address == model.Email);
                isvalidemail = true;


            }
            catch
            {
                errormessage += "invalid email\r\n";
                isvalidemail = false;
            }

            if (model.Amount == 0)
            {
                isvalidamount = false;
                errormessage += "invalid amount\r\n";

            }
            else
            {
                isvalidamount = true;
            }




            if (isvalidamount == true && isvalidemail == true)
            {

                try
                {
                    string Name = model.CardHolderName;
                    string CardNumber = model.CardNum;
                    long ExpirationYear = long.Parse(model.Expyear);
                    long ExpirationMonth = long.Parse(model.ExpMonth);
                    string CVV2 = model.CVV;
                    string Buyer_Email = model.Email;
                    int amount = (int)model.Amount;


                    Stripe.StripeConfiguration.SetApiKey("sk_test_KVelkjylnQQPOkrHSSu8gCft00dODAP1ie");

                    Stripe.CreditCardOptions card = new Stripe.CreditCardOptions();

                    card.Name = Name;

                    card.Number = CardNumber;

                    card.ExpYear = ExpirationYear;

                    card.ExpMonth = ExpirationMonth;
                    card.AddressLine1 = model.AddressLine1;
                    card.AddressLine2 = model.AddressLine2;
                    card.AddressState = model.AddressCity;
                    card.AddressCountry = model.AddressCountry;
                    card.AddressZip = model.AddressPostcode;
                    card.Cvc = CVV2;

                    // set card to token object and create token  

                    Stripe.TokenCreateOptions tokenCreateOption = new Stripe.TokenCreateOptions();

                    tokenCreateOption.Card = card;

                    Stripe.TokenService tokenService = new Stripe.TokenService();

                    Stripe.Token token = tokenService.Create(tokenCreateOption);

                    //create customer object then register customer on Stripe  

                    Stripe.CustomerCreateOptions customer = new Stripe.CustomerCreateOptions();

                    customer.Email = Buyer_Email;

                    var custService = new Stripe.CustomerService();

                    Stripe.Customer stpCustomer = custService.Create(customer);
                    //create credit card charge object with details of charge  

                    var options = new Stripe.ChargeCreateOptions
                    {

                        Amount = (int)(amount * 100),

                        //                    Amount = (int)(model.Amount * 100),
                        //                    Currency = "gbp",
                        //                    Description = "Description for test charge",
                        //                    Source = model.Token
                        Currency = "gbp",

                        ReceiptEmail = Buyer_Email,

                        Source = model.Token,
                        Description = "Description for test charge"

                    };

                    //and Create Method of this object is doing the payment execution.  

                    var service = new Stripe.ChargeService();

                    Stripe.Charge charge = service.Create(options); // This will do the Payment            }
                    return new HttpStatusCodeResult(HttpStatusCode.OK, "Success");

                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "error :  " + errormessage);
                }


            }
            else
            { 
 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "error :  " + errormessage);


            }

        }

        
 


    //private void IsValid(StripeChargeModel model)
    //{
    //    string errormessage ="";
    //    bool isvalidemail = false;
    //    bool isvalidamount = false;

    //    try
    //    {
    //        var addr = new System.Net.Mail.MailAddress(model.Email);
    //        bool emailvalid = ( addr.Address == model.Email) ;
    //        isvalidemail = true;


    //    }
    //    catch
    //    {
    //        errormessage += "invalid email";
    //        isvalidemail = false;
    //    }

    //    if (model.Amount == 0)
    //    {
    //        isvalidamount = false;
    //        errormessage += "invalid amount";

    //    }
    //    else
    //    {
    //        isvalidamount = true;
    //    }




    //    if (isvalidamount == false  ||  isvalidemail == false) {
    //        return View("Index");
    //    }
    //    else {
    //        return View("Index");
    //    }
    //}




}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<ActionResult> Charge(StripeChargeModel model)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return View(model);
    //    }
    //    StripeConfiguration.SetApiKey("pk_test_qOc7BjT6S3JesC6i5RlNevq700ptlnoJmt");

    //    Stripe.CreditCardOptions card = new Stripe.CreditCardOptions();

    //    card.Name = model.CardHolderName;

    //    card.Number = model.Token;

    //    card.ExpYear = model.;

    //    card.ExpMonth = params.ExpirationMonth;

    //    card.Cvc = params.CVV2;

    //    // set card to token object and create token  

    //    Stripe.TokenCreateOptions tokenCreateOption = new Stripe.TokenCreateOptions();

    //    tokenCreateOption.Card = card;

    //    Stripe.TokenService tokenService = new Stripe.TokenService();

    //    Stripe.Token token = tokenService.Create(tokenCreateOption);



    //    return Vi("Index");
    //}



    //        private static async Task<string> ProcessPayment(StripeChargeModel model)
    //        {
    //            return await Task.Run(() =>
    //            {
    //                StripeConfiguration.ApiKey = "pk_test_qOc7BjT6S3JesC6i5RlNevq700ptlnoJmt";

    //                var myCharge = new Stripe.ChargeCreateOptions()
    //                {
    //                    // convert the amount of £12.50 to pennies i.e. 1250
    //                    Amount = (int)(model.Amount * 100),
    //                    Currency = "gbp",
    //                    Description = "Description for test charge",
    //                    Source = model.Token

    //                };

    //                var service = new ChargeService();
    //                var stripeCharge = service.Create(myCharge);
    //                return stripeCharge.Id;
    //            });
    //        }


}
    