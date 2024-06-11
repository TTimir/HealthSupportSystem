using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;
using Microsoft.Ajax.Utilities;

namespace HealthSupportSystem.Controllers
{
    public class Forum_QuestionsController : Controller
    {
        private HealthSupportSysdbEntities db = new HealthSupportSysdbEntities();

        // GET: Forum_Questions
        public ActionResult Index()
        {
            var forum_Questions = db.Forum_Questions.Include(f => f.UserTable).Include(c => c.Forum_Answers);
            return View(forum_Questions.ToList());
        }

        // GET: Forum_Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var forum_Questions = db.Forum_Questions
                .Include(q => q.UserTable)
                .Include(q => q.Forum_Answers)
                .Include(q => q.Forum_Answers.Select(ans => ans.UserTable))
                .FirstOrDefault(m => m.QId == id);

            if (forum_Questions == null)
            {
                return HttpNotFound();
            }

            return View(forum_Questions);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAnswer(int questionId, string content)
        {
            var userId = Convert.ToInt32(Session["UserID"]);

            if (userId != null)
            {
                var answer = new Forum_Answers
                {
                    Content = content,
                    FK_UserId = userId,
                    QuestionId = questionId,
                    AnsDate = DateTime.Now
                };

                db.Forum_Answers.Add(answer);
                db.SaveChanges();

                // Redirect to the details page of the question after posting the answer
                return RedirectToAction("Details", new { id = questionId });
            }

            // Handle the case when user is not authenticated
            return RedirectToAction("Login", "Account");
        }

        // GET: Forum_Questions/Create
        public ActionResult Create()
        {
            ViewBag.FK_UserId = new SelectList(db.UserTables, "UserID", "UserName");
            return View();
        }

        // POST: Forum_Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QId,Title,Description,FK_UserId")] Forum_Questions forum_Questions)
        {
            var userId = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0;

            if (userId != 0 && ModelState.IsValid)
            {
                forum_Questions.FK_UserId = userId;
                forum_Questions.PostDate = DateTime.Now;
                db.Forum_Questions.Add(forum_Questions);
                db.SaveChanges();
                TempData["Message"] = "Your question was posted, thank you!";
                return RedirectToAction("Index");
            }

            if (userId == 0)
            {
                TempData["Message"] = "User ID not found. Please log in again.";
                return RedirectToAction("Login", "Account"); // Redirect to your login page or wherever appropriate
            }

            // If ModelState is not valid, meaning there are validation errors, or other cases where the data is empty
            TempData["Message"] = "Please fill in all required fields.";
            ViewBag.FK_UserId = new SelectList(db.UserTables, "UserID", "UserName", forum_Questions.FK_UserId);
            return View(forum_Questions);
        }

        //private int? GetCurrentUser()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        int? userId = Session["UserID"] as int?;
        //        return userId;
        //    }
        //    return null;
        //}

        // GET: Forum_Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forum_Questions forum_Questions = db.Forum_Questions.Find(id);
            if (forum_Questions == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_UserId = new SelectList(db.UserTables, "UserID", "UserName", forum_Questions.FK_UserId);
            return View(forum_Questions);
        }

        // POST: Forum_Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QId,Title,Description,FK_UserId")] Forum_Questions forum_Questions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forum_Questions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_UserId = new SelectList(db.UserTables, "UserID", "UserName", forum_Questions.FK_UserId);
            return View(forum_Questions);
        }


        public ActionResult QuestionsList()
        {
            // Get the current user's IdentityUserId
            var userId = Convert.ToInt32(Session["UserID"].ToString());

            if (userId != null)
            {
                // Retrieve only the questions created by the current user
                var userQuestions = db.Forum_Questions
                    .Where(q => q.FK_UserId == userId)
                    .Include(q => q.UserTable)
                    .Include(q => q.Forum_Answers);

                return View(userQuestions.ToList());
            }
            else
            {
                // Handle the case when the user is not authenticated or not found
                // You can redirect to a login page or display an error message
                return RedirectToAction("Login", "Account");
            }

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forum_Questions forum_Questions = db.Forum_Questions.Find(id);
            if (forum_Questions == null)
            {
                return HttpNotFound();
            }
            return View(forum_Questions);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Find the question
            Forum_Questions forum_Questions = db.Forum_Questions.Find(id);

            // Find all answers associated with the question
            var answersToDelete = db.Forum_Answers.Where(a => a.QuestionId == id);

            // Delete all associated answers
            foreach (var answer in answersToDelete)
            {
                db.Forum_Answers.Remove(answer);
            }

            // Delete the question itself
            db.Forum_Questions.Remove(forum_Questions);
            db.SaveChanges();
            return RedirectToAction("QuestionsList");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
