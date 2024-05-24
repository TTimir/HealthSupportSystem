using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthSupportSystem.Controllers
{
    public class DoctorApproveController : Controller
    {
        private HealthSupportSysdbEntities db = new HealthSupportSysdbEntities();

        // GET: DoctorApprove
        public ActionResult PendingAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var doc = (DoctorTable)Session["Doctor"];
            var pendingappointment = db.DoctorAppointTables.Where(d => d.DoctorID == doc.DoctorID && d.IsChecked == false && d.IsFeeSubmit == false && string.IsNullOrEmpty(d.DoctorComment) == true);
            return View(pendingappointment);
        }

        public ActionResult CurrentAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var doc = (DoctorTable)Session["Doctor"];
            var currappointment = db.DoctorAppointTables.Where(d => d.DoctorID == doc.DoctorID && d.IsChecked == false && d.IsFeeSubmit == true && string.IsNullOrEmpty(d.DoctorComment) == true);
            return View(currappointment);
        }

        public ActionResult CompleteAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var doc = (DoctorTable)Session["Doctor"];
            var comappointment = db.DoctorAppointTables.Where(d => d.DoctorID == doc.DoctorID && d.IsChecked == true && d.IsFeeSubmit == true && string.IsNullOrEmpty(d.DoctorComment) != true);
            return View(comappointment);
        }

        public ActionResult AllAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var doc = (DoctorTable)Session["Doctor"];
            var allappointment = db.DoctorAppointTables.Where(d => d.DoctorID == doc.DoctorID);
            return View(allappointment);
        }

        public ActionResult ChangeStatus(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var appoint = db.DoctorAppointTables.Find(id);
            ViewBag.DoctorTimeSlotID = new SelectList(db.DoctorTimeSlotTables.Where(d => d.DoctorID == appoint.DoctorID), "DoctorTimeSlotID", "Name", appoint.DoctorTimeSlotID);
            return View(appoint);
        }

        [HttpPost]
        public ActionResult ChangeStatus(DoctorAppointTable appointTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Entry(appointTable).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PendingAppoint");
            }
            ViewBag.DoctorTimeSlotID = new SelectList(db.DoctorTimeSlotTables.Where(d => d.DoctorID == appointTable.DoctorID), "DoctorTimeSlotID", "DoctorTimeSlotID", appointTable.DoctorTimeSlotID);
            return View(appointTable);
        }

        public ActionResult ProcessAppointment(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var appoint = db.DoctorAppointTables.Find(id);
            return View(appoint);
        }

        [HttpPost]
        public ActionResult ProcessAppointment(DoctorAppointTable appointTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(appointTable).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CurrentAppoint");
            }
            return View(appointTable);
        }

        public ActionResult ViewDetails(int? id)
        {
            var appointment = db.DoctorAppointTables.Find(id);
            ViewBag.Doctor = appointment.DoctorTable.Name;
            ViewBag.DocContact = appointment.DoctorTable.ClinicPhoneNo;
            ViewBag.DocEmail = appointment.DoctorTable.EmailAddress;
            ViewBag.DocComment = appointment.DoctorComment;
            ViewBag.ClinicAddress = appointment.DoctorTable.ClinicAddress;
            ViewBag.Patient = appointment.PatientTable.Name;
            ViewBag.PatientEmail = appointment.PatientTable.Email;
            ViewBag.AppointmentNo = appointment.DoctorAppointID;
            ViewBag.AppointmentDate = appointment.AppointDate;
            ViewBag.Amount = appointment.IsFeeSubmit ? "Paid" : "Not applicable";

            ViewBag.DocLogo = db.DoctorAppointTables.Find(id).DoctorTable.Photo;
            ViewBag.PatientLogo = db.DoctorAppointTables.Find(id).PatientTable.Photo;

            return View(db.DoctorAppointTables.Where(p => p.DoctorAppointID == id).ToList());
        }
    }
}