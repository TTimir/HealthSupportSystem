using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthSupportSystem.Controllers
{
    public class LabAppointStatusController : Controller
    {
        private HealthSupportSysdbEntities db = new HealthSupportSysdbEntities();

        // GET: DoctorAppointStatus
        public ActionResult PendingAppointment()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patient = (PatientTable)Session["Patient"];
            var pendingappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsComplete == false && d.IsFeeSubmit == false);
            return View(pendingappointment);
        }

        public ActionResult CurrentAppointment()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patient = (PatientTable)Session["Patient"];
            var currappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsComplete == false && d.IsFeeSubmit == true);
            return View(currappointment);
        }

        public ActionResult CompleteAppointment()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patient = (PatientTable)Session["Patient"];
            var comappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsComplete == true && d.IsFeeSubmit == true);
            return View(comappointment);
        }

        public ActionResult CancelAppointment()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patient = (PatientTable)Session["Patient"];
            var canappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && (d.IsComplete == false || d.IsFeeSubmit == false) && d.AppointDate > DateTime.Now);
            return View(canappointment);
        }

        public ActionResult AllAppointment()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var patient = (PatientTable)Session["Patient"];
            var allappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && (d.IsComplete == false || d.IsFeeSubmit == false) && d.AppointDate > DateTime.Now);
            return View(allappointment);
        }
    }
}