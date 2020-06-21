using Okta.AspNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BonusTask.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    OktaDefaults.MvcAuthenticationType);
                return new HttpUnauthorizedResult();
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult PostLogout()
        {
            return RedirectToAction("Index", "Home");

        }
    }
}