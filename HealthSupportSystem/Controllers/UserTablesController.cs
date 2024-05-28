using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;

namespace HealthSupportSystem.Controllers
{
    public class UserTablesController : Controller
    {
        private HealthSupportSysdbEntities db = new HealthSupportSysdbEntities();

        // GET: UserTables
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var userTables = db.UserTables.Include(u => u.UserTypeTable);
            return View(userTables.ToList());
        }

        // GET: UserTables/UserDetails/5
        public ActionResult UserDetails(int? id)
        {
            // Check if the user is logged in
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            // If no ID parameter is provided, try to find the ID of the logged-in user
            if (id == null)
            {
                // Get the username of the logged-in user from the session
                string username = Session["UserName"].ToString();

                // Query the database to retrieve the user ID based on the username
                var loggedInUser = db.UserTables.FirstOrDefault(u => u.UserName == username);

                // Check if the logged-in user exists
                if (loggedInUser == null)
                {
                    return HttpNotFound();
                }

                // Redirect to the UserDetails action with the ID of the logged-in user
                return RedirectToAction("UserDetails", new { id = loggedInUser.UserID });
            }

            // If an ID parameter is provided, proceed with fetching the user details
            UserTable userTable = db.UserTables.Find(id);

            if (userTable == null)
            {
                return HttpNotFound();
            }

            return View(userTable);
        }



        public ActionResult DoctorList()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            // Filter users based on UserTypeID
            var doctorList = db.DoctorTables.ToList();
            return View(doctorList);
        }

        public ActionResult PendingRequests()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            // Filter doctors based on UserTypeID and IsVerified status
            var pendingDoctors = db.UserTables.Where(u => u.UserTypeID == 2 && u.IsVerified == false).ToList();
            return View(pendingDoctors);
        }

        public ActionResult PatientList()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            // Filter patients based on UserTypeID
            var patientList = db.UserTables.Where(u => u.UserTypeID == 4).ToList();
            return View(patientList);
        }


        // GET: UserTables/Details/5
        public ActionResult Details(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTable userTable = db.UserTables.Find(id);
            if (userTable == null)
            {
                return HttpNotFound();
            }
            return View(userTable);
        }

        // GET: UserTables/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.UserTypeID = new SelectList(db.UserTypeTables, "UserTypeID", "UserType");
            return View();
        }

        // POST: UserTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserTypeID,UserName,Password,Email,ContactNo,Description,IsVerified")] UserTable userTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.UserTables.Add(userTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserTypeID = new SelectList(db.UserTypeTables, "UserTypeID", "UserType", userTable.UserTypeID);
            return View(userTable);
        }

        // GET: UserTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTable userTable = db.UserTables.Find(id);
            if (userTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserTypeID = new SelectList(db.UserTypeTables.Where(u => u.UserTypeID != 1 && u.UserTypeID != 3), "UserTypeID", "UserType", userTable.UserTypeID);
            return View(userTable);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserTypeID,UserName,Password,Email,ContactNo,Description,IsVerified")] UserTable userTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (!ModelState.IsValid)
            {
                // Log the validation errors or display them
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    // Log the error (you can replace this with your logging mechanism)
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }

                // Optionally, add a general error message
                ModelState.AddModelError("", "There are validation errors. Please check your input.");
            }
            if (ModelState.IsValid)
            {
                db.Entry(userTable).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the error (uncomment the line below after adding a logging mechanism)
                    // Log.Error(ex, "An error occurred while updating the user.");
                    ModelState.AddModelError("", "An error occurred while saving changes. Please try again.");
                }
            }
            ViewBag.UserTypeID = new SelectList(db.UserTypeTables.Where(u => u.UserTypeID != 1 && u.UserTypeID != 3), "UserTypeID", "UserType", userTable.UserTypeID);
            return View(userTable);
        }

        // GET: UserTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTable userTable = db.UserTables.Find(id);
            if (userTable == null)
            {
                return HttpNotFound();
            }
            return View(userTable);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            UserTable userTable = db.UserTables.Find(id);

            if (userTable == null)
            {
                return HttpNotFound();
            }

            // Check if the user is a patient
            if (userTable.UserTypeID == 4)
            {
                // If the user is a patient, delete associated patient records
                var patientRecords = db.PatientTables.Where(p => p.UserID == id).ToList();
                foreach (var record in patientRecords)
                {
                    db.PatientTables.Remove(record);
                }
            }
            // Check if the user is a doctor
            else if (userTable.UserTypeID == 2)
            {
                // If the user is a doctor, delete associated records in DoctorAppointTable first
                var doctorTimeSlots = db.DoctorTimeSlotTables.Where(dts => dts.DoctorID == id).ToList();
                foreach (var timeSlot in doctorTimeSlots)
                {
                    var doctorAppointments = db.DoctorAppointTables.Where(da => da.DoctorTimeSlotID == timeSlot.DoctorTimeSlotID).ToList();
                    foreach (var appointment in doctorAppointments)
                    {
                        db.DoctorAppointTables.Remove(appointment);
                    }
                    db.DoctorTimeSlotTables.Remove(timeSlot);
                }

                // Save changes to ensure DoctorAppointTable and DoctorTimeSlotTable entries are removed
                db.SaveChanges();

                // Then delete associated doctor records
                var doctorRecords = db.DoctorTables.Where(d => d.UserID == id).ToList();
                foreach (var record in doctorRecords)
                {
                    db.DoctorTables.Remove(record);
                }
            }

            // Save changes to ensure PatientTables or DoctorTables entries are removed
            db.SaveChanges();

            // Now, delete the user
            db.UserTables.Remove(userTable);
            db.SaveChanges();
            return RedirectToAction("Index");
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
