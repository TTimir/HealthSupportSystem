using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HealthSupportSystem.Controllers
{
    public class HomeController : Controller
    {
        private HealthSupportSysdbEntities db = new HealthSupportSysdbEntities();

        public ActionResult StartTemplate()
        {
            if(string.IsNullOrEmpty(Convert.ToString(Session["UserName"]))) 
            {
                return RedirectToAction("Login", "Home"); 
            }
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
            {
                return View("Login");
            }

            var user = db.UserTables.Where(u => u.Email == email && u.Password == password && u.IsVerified == true).FirstOrDefault();
            
            if (user != null)
            {
                Session["UserID"] = user.UserID;
                Session["UserTypeID"] = user.UserTypeID;
                Session["UserName"] = user.UserName;
                Session["Password"] = user.Password;
                Session["Email"] = user.Email;
                Session["ContactNo"] = user.ContactNo;
                Session["Description"] = user.Description;
                Session["IsVerified"] = user.IsVerified;
                return RedirectToAction("Index");
            }


            LogOut();
            return View("Login");
        }

        public void LogOut()
        {
            Session["UserID"] = string.Empty;
            Session["UserTypeID"] = string.Empty;
            Session["UserName"] = string.Empty;
            Session["Password"] = string.Empty;
            Session["Email"] = string.Empty;
            Session["ContactNo"] = string.Empty;
            Session["Description"] = string.Empty;
            Session["IsVerified"] = string.Empty;
        }

        public ActionResult ChangePassword()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int? userid = Convert.ToInt32(Session["UserID"].ToString());
            UserTable users = db.UserTables.Find(userid);

            if (users.Password == OldPassword)
            {
                if (NewPassword == ConfirmPassword)
                {
                    users.Password = NewPassword;
                    db.Entry(users).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.message = "Saved Successfully!";
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    ViewBag.message = "New Password and Confirm Password Not Matcehd!";
                    return View("ChangePassword");
                }
            }
            else
            {
                ViewBag.message = "Old Password is Incorrect!";
                return View("ChangePassword");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }
    }
}