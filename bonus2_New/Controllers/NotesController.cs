using bonus2_New.Models;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bonus2_New.Controllers
{
    public class NotesController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "1Vg6ZKMuhcOSyfM0B96usEwvlMXutSdJ3x6oETO2",
            BasePath = "https://fir-notes-df65c.firebaseio.com/"
        };

        FirebaseClient client;

        // GET: Notes
        public ActionResult CreateNote()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateNote(Notes notes)
        {
            try
            {
                AddNotesToDatabase(notes);
                ModelState.AddModelError(string.Empty, "Note added successfully.");


            }
            catch (Exception ex)
            {

            }
            return View();

        }

        private void AddNotesToDatabase(Notes notes)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = notes;
            PushResponse response = client.Push("Notes/", data);
            SetResponse setResponse = client.Set("Notes/" + data.title, data);
        }
    }
}