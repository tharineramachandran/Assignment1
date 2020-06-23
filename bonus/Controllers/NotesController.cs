using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bonus.Models;
using Firebase.Database;
using Firebase.Database.Query;

namespace bonus.Controllers
{
    public class NotesController : Controller
    {
        // GET: Notes/Create
        public ActionResult AddNotes()
        {
            return View();
        }

        // POST: Notes/Create
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Addnotes(   )
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

                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
