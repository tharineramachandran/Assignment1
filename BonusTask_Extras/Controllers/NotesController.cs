using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonusTask_Extras.Models;
using Firebase.Database;
using Firebase.Database.Query;

namespace BonusTask_Extras.Controllers
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
        public async System.Threading.Tasks.Task<ActionResult> AddNotesAsync(NotesModel notesModel)
        {
            var firebaseClient = new FirebaseClient("https://fir-notes-df65c.firebaseio.com/");

            try
            {
                string title = notesModel.Title;
                string name = notesModel.Note;

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
