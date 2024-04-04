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
    public class LabTimeSlotTablesController : Controller
    {
        private HealthSupportSysdbEntities db = new HealthSupportSysdbEntities();

        // GET: LabTimeSlotTables
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var lab = (LabTable)Session["Lab"];
            var labTimeSlotTables = db.LabTimeSlotTables.Include(l => l.LabTable).Where(l => l.LabID == lab.LabID);
            return View(labTimeSlotTables.ToList());
        }

        // GET: LabTimeSlotTables/Details/5
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
            LabTimeSlotTable labTimeSlotTable = db.LabTimeSlotTables.Find(id);
            if (labTimeSlotTable == null)
            {
                return HttpNotFound();
            }
            return View(labTimeSlotTable);
        }

        // GET: LabTimeSlotTables/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.LabID = new SelectList(db.LabTables, "LabID", "Name");
            return View();
        }

        // POST: LabTimeSlotTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LabTimeSlotTable labTimeSlotTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var lab = (LabTable)Session["Lab"];
            labTimeSlotTable.LabID = lab.LabID;
            if (ModelState.IsValid)
            {
                var findtiemslot = db.LabTimeSlotTables.Where(t => t.LabID == lab.LabID && t.FromTime == labTimeSlotTable.FromTime && t.ToTime == labTimeSlotTable.ToTime).FirstOrDefault();
                if (findtiemslot == null)
                {
                    db.LabTimeSlotTables.Add(labTimeSlotTable);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already in List, Please Check!";
                }
            }

            ViewBag.LabID = new SelectList(db.LabTables, "LabID", "Name", labTimeSlotTable.LabID);
            return View(labTimeSlotTable);
        }

        // GET: LabTimeSlotTables/Edit/5
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
            LabTimeSlotTable labTimeSlotTable = db.LabTimeSlotTables.Find(id);
            if (labTimeSlotTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.LabID = new SelectList(db.LabTables, "LabID", "Name", labTimeSlotTable.LabID);
            return View(labTimeSlotTable);
        }

        // POST: LabTimeSlotTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LabTimeSlotTable labTimeSlotTable)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var findtiemslot = db.LabTimeSlotTables.Where(t => t.LabID == labTimeSlotTable.LabID && t.FromTime == labTimeSlotTable.FromTime && t.ToTime == labTimeSlotTable.ToTime && labTimeSlotTable.LabTimeSlotID != labTimeSlotTable.LabTimeSlotID).FirstOrDefault();
                if (findtiemslot == null)
                {
                    db.Entry(labTimeSlotTable).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "Already in List, Please Check!";
                }
            }
            ViewBag.LabID = new SelectList(db.LabTables, "LabID", "Name", labTimeSlotTable.LabID);
            return View(labTimeSlotTable);
        }

        // GET: LabTimeSlotTables/Delete/5
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
            LabTimeSlotTable labTimeSlotTable = db.LabTimeSlotTables.Find(id);
            if (labTimeSlotTable == null)
            {
                return HttpNotFound();
            }
            return View(labTimeSlotTable);
        }

        // POST: LabTimeSlotTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            LabTimeSlotTable labTimeSlotTable = db.LabTimeSlotTables.Find(id);
            db.LabTimeSlotTables.Remove(labTimeSlotTable);
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
