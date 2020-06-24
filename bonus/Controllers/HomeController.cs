using Firebase.Database;
using Firebase.Database.Query;
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

namespace bonus.Controllers
{
    public class HomeController : Controller
    {
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
        public async System.Threading.Tasks.Task<ActionResult> Addnotes()
        {

            var firebaseClient = new FirebaseClient("https://fir-notes-df65c.firebaseio.com/");

            try
            {
                string title = Request.Form["title"];
                string name = Request.Form["note"];
                var  file = Request.Files["image"];
                DeepAI_API api = new DeepAI_API(apiKey: "7cc97bee-3a97-44bd-bb9c-a5cad72bc4df" );

                StandardApiResponse resp = api.callStandardApi("facial-expression-recognition", new
                {
                    image = name,
                });
                Console.Write(api.objectAsJsonString(resp));

 

                DeepAI_API apei = new DeepAI_API(apiKey: "7cc97bee-3a97-44bd-bb9c-a5cad72bc4df");
                string path = Path.Combine(Server.MapPath("~/"),
                                       Path.GetFileName(file.FileName));
                file.SaveAs(path); 
                StandardApiResponse rty = apei.callStandardApi("facial-expression-recognition", new
                {
                    image =  System.IO.File.OpenRead(path),
            });
                Console.Write(apei.objectAsJsonString(rty));




                var result = await firebaseClient
                    .Child("Notes")
                    .Child(title)
                    .PostAsync(name);

                return Json ("asfafd");
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        public ActionResult Addmood()
        {

            
            try
            {
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








                        Response.StatusCode = 200;
                        return Json("Successful");
                    }else
                    {
                        Response.StatusCode = 400;
                        return Json("Could not track your emotion from the image, Try uploading a different image ");

                    }
                }
                else {
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
