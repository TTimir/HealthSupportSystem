using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthSupportSystem.Controllers
{
    public class DoctorApproveController : Controller
    {
        // GET: DoctorApprove
        public ActionResult PendingAppoint()
        {
            return View();
        }
    }
}