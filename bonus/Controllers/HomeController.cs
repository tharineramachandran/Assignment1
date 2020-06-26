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
using Firebase.Database;
using Firebase.Database.Query;
using FireSharp.Extensions;
using Newtonsoft.Json;

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
 
    }
}
