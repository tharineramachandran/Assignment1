using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Predictions;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using RestSharp;
using task8_clarifai.Controllers;

namespace task8_clarifai.Controllers
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
            var file = Request.Files["image"];
            var time = DateTime.Now;
            String timestamp = time.ToString("dd_MM_yyyy_hh_mm_ss");
            if (file != null)
            {

                string path = Path.Combine(Server.MapPath("~/"),
                                          Path.GetFileName(file.FileName));
                file.SaveAs(path);


                bool uploaded = amazon(path, timestamp);
                var image = Image.FromFile(path);

                image.Dispose();



                if (uploaded)
                {
                    try
                    {
                        //var image = Request.Files["image"];// With `CLARIFAI_API_KEY` defined as an environment variable


                        var awsimage = "https://task4-csc.s3.amazonaws.com/" + file.FileName;


                        // When passed in as a string
                        var client = new ClarifaiClient("f83d8725b08444b8be53e41402d013d3");

                        var response = await client.Predict<Concept>(
                           "receipt",
                           new ClarifaiURLImage(awsimage),
                           modelVersionID: "d737a7588cd849c7b47effb53a3ad9de")
                         .ExecuteAsync();




                        foreach (var concept in response.Get().Data)
                        {
                            double value = (double)concept.Value;
                            if (concept.Name == "receipt" && value >= 0.70)
                            {
                                isReceipt = true;
                            }

                        }

                        if (isReceipt == true)
                        {
                            var nanoclient = new RestClient("https://app.nanonets.com/api/v2/OCR/Model/e252887c-03d4-44d1-b433-3554b69511fd/LabelUrls/");
                            var request = new RestRequest(Method.POST);
                            request.AddHeader("authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("TLddl25FfwjnRYjBBluH3UKy-l9Mt5cy:")));

                            request.AddHeader("accept", "application/x-www-form-urlencoded");
                            request.AddParameter("urls", awsimage);
                            IRestResponse nanoresponse = nanoclient.Execute(request);

                            JObject json = JObject.Parse(nanoresponse.Content);
                            var prediction = json["result"][0]["prediction"];

                            foreach (var predict in prediction)
                            {
                                String label = (string)predict["label"];

                                if (label.ToLower().Contains("total"))
                                {
                                    total = (string)predict["ocr_text"];


                                }
                            }

                        }
                        else
                        {


                        }

                    }
                    catch (Exception e)
                    {
                        Response.StatusCode = 400;
                        return Json("an error occurred");

                    }
                    String message = "";
                    if (isReceipt == true)
                    {

                        if (total.IsNullOrWhiteSpace())
                        {
                            message = "The image is a receipt but no total value was predicted";
                        }
                        else
                        {
                            message = "The image is a receipt and the total value is " + total;

                        }
                    }
                    else
                    {
                        message = "The image is not a receipt";

                    }


                    Response.StatusCode = 200;
                    return Json(message);
                } 
                 
                else { 
                    Response.StatusCode = 400; 
                    return Json("Failed to check image"); }
            }

            Response.StatusCode = 400;
            return Json("Add Image");
        }

        private bool amazon(string path, string timestamp)
        {
            var bucketName = "task4-csc";

            // Specify your bucket region (an example region is shown).
            RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
            IAmazonS3 s3Client;


            s3Client = new AmazonS3Client("ASIATS2W2CQO44S5Y677", "XW4vk0ygyfRuRWTa8SEA+UcgyRhrp3W4R7Cp4ymX", "FwoGZXIvYXdzEOz//////////wEaDC3PcX0BSHD7O4sJSyLOAXYYJdbfpsj7Ux4budW64S1/TpdTeWquk7G5e69EAeLlGxinZQuDekfJYVcZhwT0pmDiqfpcNgQLtqET4vibyDnPKwQ1w9Z5QOIma7Mk9MjSo1HR6NpIbYBIrpihvW7wHUBfhLxPetMz6g/VhDSHZHiPQi7OdiY7UG6ptelTu5VRCyfxboyEJlNC7CcJf47qi0e73QdIsseEomn157ZEv4HWVhXSn7EKqP6Ss7Z5sMLPEYIshzP7o4/Y8qfQ6IPPczLmBy6iH2uIyup2e5k4KKP4w/cFMi2S86QDutRnl8MXBobdrfBuTKhfgvaMy2i//T6a1//65YLmLKiHmCSkDdwc0pg="
                ,                 bucketRegion);



            try
            {
                var fileTransferUtility =
                    new TransferUtility(s3Client);

                // Option 1. Upload a file. The file name is used as the object key name.
                fileTransferUtility.UploadAsync(path, bucketName);
                Console.WriteLine("Upload 1 completed");
                 
                return true;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                return false;
            }
        }
    }
}
