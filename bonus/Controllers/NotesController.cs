using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bonus.Models;
using DeepAI;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json.Linq;

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
        public async System.Threading.Tasks.Task<ActionResult> Addnotes()
        {

            var firebaseClient = new FirebaseClient("https://fir-notes-df65c.firebaseio.com/");

            try
            {
                string title = Request.Form["title"];
                string name = Request.Form["note"];
                var file = Request.Files["image"];


                NotesModel model = new NotesModel();
                model.Title = title;
                model.timestamp = Convert.ToString(DateTime.Now);
                model.Note = name;



                var result = await firebaseClient
                    .Child("Notes")
                    .PostAsync(model);

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}
