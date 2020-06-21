using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;
using BonusTask.Models;
using System.Globalization;
using System.IdentityModel.Claims;
using System.Security.Claims;

namespace BonusTask.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> About()
        {
            //Get Okta user data
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<System.Security.Claims.Claim> claims = identity.Claims;
            var currentLoginTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            //Save non identifying data to Firebase
            var currentUserLogin = new LoginData() { Timestamp = currentLoginTime };
            var firebaseClient = new FirebaseClient("https://fir-login-e9f4c.firebaseio.com/");
            var result = await firebaseClient
              .Child("Users/" + "/Logins")
              .PostAsync(currentUserLogin);

            //Retrieve data from Firebase
            var dbLogins = await firebaseClient
              .Child("Users")
              .Child("Logins")
              .OnceAsync<LoginData>();

            var timestampList = new List<DateTime>();

            //Convert JSON data to original datatype
            foreach (var login in dbLogins)
            {
                timestampList.Add(DateTime.Now);
            }

            //Pass data to the view
            ViewBag.Logins = timestampList.OrderByDescending(x => x);
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}