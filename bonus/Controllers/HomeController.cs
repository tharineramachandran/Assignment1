using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ParallelDots;
using DeepAI;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using bonus.Models;
using FireSharp.Interfaces;
using FireSharp;
using FireSharp.Response;
using System.Threading.Tasks;
using FireSharp.Config;

namespace bonus.Controllers
{
    public class HomeController : Controller
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
          String   BasePath = "https://refundcsc.firebaseio.com/";

            client = new Firebase.Database.FirebaseClient(BasePath);

            FirebaseResponse response = await client.GetAsync("Subscription/");
            JObject jsonobject = JObject.Parse(response.Body);
            List<Models.MoodModel> parsedFields = new List<Models.MoodModel>();

            foreach (var objectrefund in jsonobject)
            {


                var objectp = objectrefund.Value;
                String customerid = (string)objectp["customerName"];
                String price = (string)objectp["PriceId"];
                Models.MoodModel refund = new Models.MoodModel();
                refund.Mood = customerid;
                refund.Timestamp = price;

                parsedFields.Add(refund);

            }


            return Json(parsedFields);
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult Mood()
        {
            ViewBag.Title = "Mood Page";

            return View();
        }

        public async System.Threading.Tasks.Task<ActionResult> Addmood()
        {
            try
            {
                var firebaseClient = new FirebaseClient("https://fir-notes-df65c.firebaseio.com/");
                var file = Request.Files["image"];
                var timestamp = DateTime.Now.ToString("DD/MM/YY");
                if (file != null)
                {

                    DeepAI_API apei = new DeepAI_API(apiKey: "7cc97bee-3a97-44bd-bb9c-a5cad72bc4df");
                    string path = Path.Combine(Server.MapPath("~/"),
                                           Path.GetFileName(file.FileName));
                    if (!System.IO.File.Exists(path))
                    {
                        file.SaveAs(path);
                    }
                    var fileread = System.IO.File.OpenRead(path);
                    StandardApiResponse rty = apei.callStandardApi("facial-expression-recognition", new
                    {
                        image = fileread,
                    });
                    var returnjson = apei.objectAsJsonString(rty);
                    fileread.Close();
                    /// Add Firebase that uploads the results 
                    JObject json = JObject.Parse(returnjson);
                    var moodsdetected = json["output"]["expressions"];
                    if (moodsdetected.HasValues)
                    {
                        string bestmood = "";
                        double bestconfidence = 0;
                        foreach (var mood in moodsdetected)
                        {
                            var confidence = double.Parse(mood["confidence"].ToString());
                            var emotion = mood["emotion"].ToString();
                            if (confidence > bestconfidence)
                            {

                                bestconfidence = confidence;
                                bestmood = emotion;
                            }

                        }

                        Dispose();

                        System.IO.File.Delete(path);
                        /// Add Firebase that uploads the results  bestmood  n timestamp   
                        /// 

                        MoodModel model = new MoodModel();
                        model.Timestamp = Convert.ToString(DateTime.Now);
                        model.Mood = bestmood;



                        var result = await firebaseClient
                            .Child("Mood")
                            .PostAsync(model);

                        Response.StatusCode = 200;
                        return Json("Successful");
                    }
                    else
                    {
                        Response.StatusCode = 400;
                        return Json("Could not track your emotion from the image, Try uploading a different image ");

                    }
                }
                else
                {
                    Response.StatusCode = 400;
                    return Json("Add an image");

                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json("an error has occurred");
            }
        }
    }
}
