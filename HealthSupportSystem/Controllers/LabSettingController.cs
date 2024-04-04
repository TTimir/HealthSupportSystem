using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace HealthSupportSystem.Controllers
{
    public class LabSettingController : Controller
    {
        private HealthSupportSysdbEntities db = new HealthSupportSysdbEntities();

        // GET: LabSetting
        public ActionResult LabAllTest()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (Session["Lab"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var lab = (LabTable)Session["Lab"];
            var testlist = db.LabTestTables.Where(l => l.LabID == lab.LabID).ToList();

            return View(testlist);
        }

        public ActionResult AddTest()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddTest(LabTestTable test)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (Session["Lab"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var lab = (LabTable)Session["Lab"];

            test.LabID = lab.LabID;
            if (ModelState.IsValid)
            {
                var findlab = db.LabTestTables.Where(l => l.Name == test.Name).FirstOrDefault();
                if (findlab == null)
                {
                    db.LabTestTables.Add(test);
                    db.SaveChanges();
                    return RedirectToAction("LabAllTest");
                }
                else
                {
                    ViewBag.Message = "Already Registered!";
                }
            }
            return View(test);
        }

        public ActionResult TestDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            Session["LabTestID"] = id;
            var detaillist = db.LabTestDetailsTables.Where(l => l.LabTestID == id).ToList();
            return View(detaillist);
        }

        public ActionResult AddTestDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddTestDetails(LabTestDetailsTable testDetails)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (Session["Lab"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var lab = (LabTable)Session["Lab"];

            testDetails.LabID = lab.LabID;
            int testid = (int)Session["LabTestID"];
            testDetails.LabTestID = testid;

            if (ModelState.IsValid)
            {
                var finddetails = db.LabTestDetailsTables.Where(l => l.Name == testDetails.Name).FirstOrDefault();
                if (finddetails == null)
                {
                    db.LabTestDetailsTables.Add(testDetails);
                    db.SaveChanges();
                    return RedirectToAction("TestDetails", new { id = testid });
                }
                else
                {
                    ViewBag.Message = "Already Registered!";
                }
            }
            return View(testDetails);
        }

        public ActionResult EditTest(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var labtest = db.LabTestTables.Find(id);
            return View(labtest);
        }

        [HttpPost]
        public ActionResult EditTest(LabTestTable test)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (Session["Lab"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var findlab = db.LabTestTables.Where(l => l.Name == test.Name && l.LabTestID != test.LabTestID).FirstOrDefault();
                if (findlab == null)
                {
                    db.Entry(test).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("LabAllTest");
                }
                else
                {
                    ViewBag.Message = "Already Registered!";
                }
            }
            return View(test);
        }

        public ActionResult EditTestDetails(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var details = db.LabTestDetailsTables.Find(id);
            return View(details);
        }

        [HttpPost]
        public ActionResult EditTestDetails(LabTestDetailsTable testDetails)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (Session["Lab"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var finddetails = db.LabTestDetailsTables.Where(l => l.Name == testDetails.Name && l.LabTestDetailID != testDetails.LabTestDetailID).FirstOrDefault();
                if (finddetails == null)
                {
                    db.Entry(testDetails).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("TestDetails", new { id = testDetails.LabTestID });
                }
                else
                {
                    ViewBag.Message = "Already Registered!";
                }
            }
            return View(testDetails);
        }
    }
}