using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bonus.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        
        public async System.Threading.Tasks.Task<ActionResult> Addnotes()
        {

            var firebaseClient = new FirebaseClient("https://fir-notes-df65c.firebaseio.com/");

            try
            {
                string title = Request.Form["title"];
                string name = Request.Form["note"];


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
    }
}
