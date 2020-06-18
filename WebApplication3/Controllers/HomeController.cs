using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Predictions;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using RestSharp;
using Task5.Controllers;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public async System.Threading.Tasks.Task<ActionResult> Clarifai()
        {
            String total = "";
            bool isReceipt = false;
            try {
                //var image = Request.Files["image"];// With `CLARIFAI_API_KEY` defined as an environment variable


                 var image = Request.Form["image"];


                 // When passed in as a string
                var client = new ClarifaiClient("f83d8725b08444b8be53e41402d013d3"); 

                var response = await client.Predict<Concept>(
                   "receipt",
                   new ClarifaiURLImage(image),
                   modelVersionID: "d737a7588cd849c7b47effb53a3ad9de")
                 .ExecuteAsync();


                
 
                foreach (var concept in response.Get().Data)
                {
                    double value = (double)concept.Value;
                        if (concept.Name == "receipt" && value >= 0.70) {
                        isReceipt = true;
                    }

                }

                if (isReceipt == true)
                {
                    var nanoclient = new RestClient("https://app.nanonets.com/api/v2/OCR/Model/e252887c-03d4-44d1-b433-3554b69511fd/LabelUrls/");
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("TLddl25FfwjnRYjBBluH3UKy-l9Mt5cy:")));
                     
                    request.AddHeader("accept", "application/x-www-form-urlencoded");
                    request.AddParameter("urls", image);
                    IRestResponse nanoresponse = nanoclient.Execute(request);

                    JObject json = JObject.Parse(nanoresponse.Content); 
                    var prediction = json["result"][0]["prediction"];
                     
                    foreach (var predict in prediction) {
                        String label = (string)predict["label"];

                        if (label.ToLower().Contains("total")) {
                            total = (string)predict["ocr_text"];
                        
                        
                        }
                    }
                     
                }
                else {

                    
                }
                 
            }
            catch (Exception e)
            {
                return Json("an Error occurred");

            }
            String message = "";
            if (isReceipt == true) {

                if (total.IsNullOrWhiteSpace())
                {
                    message = "The image is a receipt but no total value was predicted";
                }
                else {
                    message = "The image is a receipt and the total value is " + total;

                }
            } else {
                message = "The image is not a receipt and  but no total value was predicted";

            } 
             
            
            return Json(message);
        }

    }
}
