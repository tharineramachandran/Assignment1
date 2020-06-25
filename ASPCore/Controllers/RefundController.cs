using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCore.Models;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stripe;
using Task6_ANDRE.Models;

namespace ASPCore.Controllers
{
    public class RefundController : Controller
    {

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "WlzsaHKFjCMdBFa1lEvOr36PR2oFvEZDEPFPCJkG",
            BasePath = "https://refundcsc.firebaseio.com/"
        };

        IFirebaseClient client;

        // GET: RefundController/Create
        public async Task<ActionResult> RefundTable()
        {
            client = new FirebaseClient(config);

            FirebaseResponse response = await client.GetAsync("Subscription/");
            JObject jsonobject = JObject.Parse(response.Body);
            List<Models.Refund> parsedFields = new List<Models.Refund>();

            foreach (var objectrefund in jsonobject)
            {


                var objectp = objectrefund.Value;
                String customerid = (string)objectp["customerName"];
                String price = (string)objectp["PriceId"];
                Models.Refund refund = new Models.Refund();
                refund.SubscriptionName = customerid;
                refund.RefundAmount = price;

                parsedFields.Add(refund);

            }


            return Json(parsedFields);
        }
        public ActionResult Index()
        {
            return View();
        }

        // POST: RefundController/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RefundTable(ChargeViewModel chargeViewModel)
        {
            try
            {
                var options = new RefundCreateOptions
                {
                    Charge = "ch_1GuKQ0DUISIWU8NfwrBXYafU",
                };
                var service = new RefundService();
                service.Create(options);

                DeleteSubscriptionAsync(chargeViewModel);

                return View();
            }
            catch
            {
                return View();
            }
        }

        private async Task DeleteSubscriptionAsync(ChargeViewModel chargeViewModel)
        {
            client = new FirebaseClient(config);

            FirebaseResponse response = await client.DeleteAsync("Subscription/");

        }
    }
}
