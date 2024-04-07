using DatabaseLayer;
using HealthSupportSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthSupportSystem.Controllers
{
    public class LabApproveController : Controller
    {
        private HealthSupportSysdbEntities db = new HealthSupportSysdbEntities();

        // GET: LabApprove
        public ActionResult PendingAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var lab = (LabTable)Session["Lab"];
            var pendingappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID && d.IsComplete == false && d.IsFeeSubmit == false);
            return View(pendingappointment);
        }

        public ActionResult CurrentAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var lab = (LabTable)Session["Lab"];
            var currappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID && d.IsComplete == false && d.IsFeeSubmit == true);
            return View(currappointment);
        }

        public ActionResult CompleteAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var lab = (LabTable)Session["Lab"];
            var comappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID && d.IsComplete == true && d.IsFeeSubmit == true);
            return View(comappointment);
        }

        public ActionResult AllAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var lab = (LabTable)Session["Lab"];
            var allappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID);
            return View(allappointment);
        }

        public ActionResult ChangeStatus(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var appoint = db.LabAppointTables.Find(id);
            ViewBag.LabTimeSlotID = new SelectList(db.LabTimeSlotTables.Where(d => d.LabID == appoint.LabID), "LabTimeSlotID", "Name", appoint.LabTimeSlotID);
            return View(appoint);
        }

        [HttpPost]
        public ActionResult ChangeStatus(LabAppointTable appointTable)
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
            ViewBag.LabTimeSlotID = new SelectList(db.LabTimeSlotTables.Where(d => d.LabID == appointTable.LabID), "LabTimeSlotID", "LabTimeSlotID", appointTable.LabTimeSlotID);
            return View(appointTable);
        }

        public ActionResult ProcessAppoint(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            List<PatientAppointmentReportMV> detailslist = new List<PatientAppointmentReportMV>();
            var appoint = db.LabAppointTables.Find(id);
            var testdetails = db.LabTestDetailsTables.Where(p => p.LabTestID == appoint.LabTestID);
            foreach (var item in testdetails)
            {
                var details = new PatientAppointmentReportMV()
                {
                    DetailName = item.Name,
                    LabAppointID = appoint.LabAppointID,
                    LabTestDetailID = item.LabTestDetailID,
                    MaxValue = item.MaxValue,
                    MinValue = item.MinValue,
                    PatientValue = 0
                };
                detailslist.Add(details);
            }

            ViewBag.TestName = appoint.LabTestTable.Name;
            return View(detailslist);
        }

        [HttpPost]
        public ActionResult ProcessAppoint(FormCollection collection)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            string[] LabTestDetailID = collection["item.LabTestDetailID"].Split(',');
            string[] LabAppointID = collection["item.LabAppointID"].Split(',');
            string[] DetailName = collection["item.DetailName"].Split(',');
            string[] MinValue = collection["item.MinValue"].Split(',');
            string[] MaxValue = collection["item.MaxValue"].Split(',');
            string[] PatientValue = collection["item.PatientValue"].Split(',');
            List<PatientAppointmentReportMV> detailslist = new List<PatientAppointmentReportMV>();
            bool issubmit = false;
            for (int i = 0; i < LabTestDetailID.Length; i++)
            {
                var details = new PatientAppointmentReportMV()
                {
                    DetailName = DetailName[i],
                    LabAppointID = Convert.ToInt32(LabAppointID[i]),
                    LabTestDetailID = Convert.ToInt32(LabTestDetailID[i]),
                    MaxValue = Convert.ToInt32(MaxValue[i]),
                    MinValue = Convert.ToInt32(MinValue[i]),
                    PatientValue = Convert.ToInt32(PatientValue[i])
                };
                if (details.PatientValue > 0)
                {
                    issubmit = true;
                }
                detailslist.Add(details);
            }
            if (issubmit == true)
            {

                foreach (var item in detailslist)
                {
                    var patdetails = new PatientTestDetailTable()
                    {
                        LabAppointID = item.LabAppointID,
                        LabTestDetailID = item.LabTestDetailID,
                        PatientValue = item.PatientValue
                    };
                    db.PatientTestDetailTables.Add(patdetails);
                    db.SaveChanges();
                }
                int appointid = Convert.ToInt32(LabAppointID[0]);
                var appoint = db.LabAppointTables.Find(appointid);
                appoint.IsComplete = true;
                db.Entry(appoint).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PendingAppoint");
            }
            else
            {
                ViewBag.Message = "Please Enter Patient Test Details!";
            }

            return View(detailslist);
        }

        public ActionResult ViewDetails(int? id)
        {
            ViewBag.TestName = db.LabAppointTables.Find(id).LabTestTable.Name;
            ViewBag.Lab = db.LabAppointTables.Find(id).LabTable.Name;
            ViewBag.LabContact = db.LabAppointTables.Find(id).LabTable.ContactNo;
            ViewBag.LabEmail = db.LabAppointTables.Find(id).LabTable.EmailAddress;
            ViewBag.LabAddress = db.LabAppointTables.Find(id).LabTable.PermanentAddress;
            ViewBag.Patient = db.LabAppointTables.Find(id).PatientTable.Name;
            ViewBag.PatientEmail = db.LabAppointTables.Find(id).PatientTable.Email;
            ViewBag.AppointmentNo = db.LabAppointTables.Find(id).TransectionNo;
            ViewBag.AppointmentDate = db.LabAppointTables.Find(id).AppointDate;

            ViewBag.LabLogo = db.LabAppointTables.Find(id).LabTable.Photo;
            ViewBag.PatientLogo = db.LabAppointTables.Find(id).PatientTable.Photo;

            return View(db.PatientTestDetailTables.Where(p => p.LabAppointID == id).ToList());
        }

    }
}