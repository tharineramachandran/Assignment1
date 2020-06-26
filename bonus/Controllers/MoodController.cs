using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;  
using Newtonsoft.Json;
using bonus.Models; 
using Firebase.Database;
using Firebase.Database.Query;
using DeepAI;
using Newtonsoft.Json.Linq;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace bonus.Controllers
{
    public class MoodController : Controller
    {

       

        // GET: RefundController/Create
        [HttpGet]
        public async Task<ActionResult> get()
        {
            var firebaseClient = new FirebaseClient("https://fir-notes-df65c.firebaseio.com/");


            var items = await firebaseClient
          .Child("Mood")
          .OrderByKey()
          .OnceAsync<MoodModel>();

            //Do whatever you need with items such as
            List<MoodModel> parsedFields = new List<MoodModel>();
            foreach (var item in items)
            { 
                parsedFields.Add(item.Object);
            } 
            

            return Json(parsedFields, JsonRequestBehavior.AllowGet );
        }
        public ActionResult Index()
        {
            return View();
        }

        // POST: RefundController/Create

        public async System.Threading.Tasks.Task<ActionResult> add()
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
