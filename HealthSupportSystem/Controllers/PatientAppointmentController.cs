using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthSupportSystem.Controllers
{
    public class PatientAppointmentController : Controller
    {
        private HealthSupportSysdbEntities db = new HealthSupportSysdbEntities();

        // GET: PatientAppointment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllDoctors()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var doclist = db.DoctorTables.Where(d => d.UserTable.IsVerified == true);
            return View(doclist);
        }
        public ActionResult GetAllLabs()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var lablist = db.LabTables.Where(d => d.UserTable.IsVerified == true);
            return View(lablist);
        }
    }
}