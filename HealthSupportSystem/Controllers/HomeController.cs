using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BCrypt.Net;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Numerics;

namespace HealthSupportSystem.Controllers
{
    public class HomeController : Controller
    {
        private HealthSupportSysdbEntities db = new HealthSupportSysdbEntities();

        public ActionResult StartTemplate()
        {
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
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                return View("Login");
            }

            var user = db.UserTables.Where(u => u.Email == email && u.Password == password && u.IsVerified == true).FirstOrDefault();

            if (user != null)
            {
                Session["UserID"] = user.UserID;
                Session["UserTypeID"] = user.UserTypeID;
                Session["UserName"] = user.UserName;
                //Session["Password"] = user.Password;
                Session["Email"] = user.Email;
                Session["ContactNo"] = user.ContactNo;
                Session["Description"] = user.Description;
                Session["IsVerified"] = user.IsVerified;

                if (user.UserTypeID == 2) // Doctor
                {
                    var doc = db.DoctorTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    Session["Doctor"] = doc;
                }
                else if (user.UserTypeID == 3) // Lab
                {
                    var lab = db.LabTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    Session["Lab"] = lab;
                }
                else if (user.UserTypeID == 4) // Patient
                {
                    var patient = db.PatientTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    Session["Patient"] = patient;
                }
                return RedirectToAction("Index");
            }

            user = db.UserTables.Where(u => u.Email == email && u.Password == password && u.IsVerified == false).FirstOrDefault();
            if (user != null)
            {
                ViewBag.Message = "Email Already Registered, Please Enter Profile Details!";

                Session["User"] = user;
                if (user.UserTypeID == 2) // Doctor
                {
                    var doc = db.DoctorTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    if (doc == null)
                    {
                        return RedirectToAction("AddDoctor");
                    }
                    ViewBag.Message = "Account is Under Review!";
                }
                else if (user.UserTypeID == 3) // Lab
                {
                    var lab = db.LabTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    if (lab == null)
                    {
                        return RedirectToAction("AddLab");
                    }
                    return RedirectToAction("AddLab");
                }
                else if (user.UserTypeID == 4) // Patient
                {
                    var patient = db.PatientTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    if (patient == null)
                    {
                        return RedirectToAction("AddPatient");
                    }
                    ViewBag.Message = "Account is Under Review!";
                }
            }
            else
            {
                ViewBag.message = "Username and Password is incorrect!";
            }

            LogOut();
            return View("Login");
        }

        public void LogOut()
        {
            Session["UserID"] = string.Empty;
            Session["UserTypeID"] = string.Empty;
            Session["UserName"] = string.Empty;
            //Session["Password"] = string.Empty;
            Session["Email"] = string.Empty;
            Session["ContactNo"] = string.Empty;
            Session["Description"] = string.Empty;
            Session["IsVerified"] = string.Empty;

            Session.Abandon();

            // Clear authentication cookie
            HttpCookie authCookie = new HttpCookie("User_SessionId", string.Empty);
            authCookie.Expires = DateTime.Now.AddMonths(-1);
            Response.Cookies.Add(authCookie);
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
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int? userid = Convert.ToInt32(Session["UserID"].ToString());
            UserTable users = db.UserTables.Find(userid);

            if (users != null && users.Password == OldPassword)
            {
                if (NewPassword == ConfirmPassword)
                {
                    if (!IsPasswordValid(NewPassword))
                    {
                        ViewBag.Message = "Password does not meet complexity requirements.";
                        return View("ChangePassword");
                    }

                    users.Password = NewPassword;
                    db.Entry(users).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.message = "Password changed successfully!";
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

        }

        public ActionResult CreateUser()
        {
            ViewBag.UserTypeID = new SelectList(db.UserTypeTables.Where(u => u.UserTypeID != 1 && u.UserTypeID != 3), "UserTypeID", "UserType", "0");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(UserTable user, string password)
        {
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    if (!IsPasswordValid(password))
                    {
                        ViewBag.Message = "Password does not meet complexity requirements.";
                        ViewBag.UserTypeID = new SelectList(db.UserTypeTables.Where(u => u.UserTypeID != 1 && u.UserTypeID != 3), "UserTypeID", "UserType", "0");
                        return View("CreateUser");
                    }

                    var finduser = db.UserTables.Where(u => u.Email == user.Email).FirstOrDefault();
                    if (finduser == null)
                    {
                        finduser = db.UserTables.Where(u => u.Email == user.Email && u.IsVerified == false).FirstOrDefault();
                        if (finduser != null)
                        {
                            ViewBag.Message = "Email Already Registered, Please Enter Profile Details!";
                            Session["User"] = finduser;

                            if (finduser.UserTypeID == 2) // Doctor
                            {
                                var doc = db.DoctorTables.Where(u => u.UserID == finduser.UserID).FirstOrDefault();
                                if (doc == null)
                                {
                                    return RedirectToAction("AddDoctor");
                                }
                                ViewBag.Message = "Account is Under Review!";
                            }
                            else if (finduser.UserTypeID == 3) // Lab
                            {
                                var lab = db.LabTables.Where(u => u.UserID == finduser.UserID).FirstOrDefault();
                                if (lab == null)
                                {
                                    return RedirectToAction("AddLab");
                                }
                                return RedirectToAction("AddLab");
                            }
                            else if (finduser.UserTypeID == 4) // Patient
                            {
                                var patient = db.PatientTables.Where(u => u.UserID == finduser.UserID).FirstOrDefault();
                                if (patient == null)
                                {
                                    return RedirectToAction("AddPatient");
                                }
                                ViewBag.Message = "Account is Under Review!";
                            }
                        }
                        else
                        {
                            if (user.UserTypeID == 2) // Doctor
                            {
                                user.IsVerified = false;
                            }
                            else if (user.UserTypeID == 3) // Lab
                            {
                                user.IsVerified = false;
                            }
                            else if (user.UserTypeID == 4) // Patient
                            {
                                user.IsVerified = true;
                            }
                            else if (user.UserTypeID == 1) // Admin
                            {
                                user.IsVerified = false;
                            }
                            user.Password = password;

                            db.UserTables.Add(user);
                            db.SaveChanges();

                            Session["User"] = user;

                            if (user.UserTypeID == 2) // Doctor
                            {
                                return RedirectToAction("AddDoctor");
                            }
                            else if (user.UserTypeID == 3) // Lab
                            {
                                return RedirectToAction("AddLab");
                            }
                            else if (user.UserTypeID == 4) // Patient
                            {
                                return RedirectToAction("AddPatient");
                            }
                            else if (user.UserTypeID == 1) // Admin
                            {
                                ViewBag.Message = "Account is Under Review!";
                            }
                        }
                    }
                }

            }
            else
            {
                ViewBag.Message = "Please Provide Correct Details!";
            }

            ViewBag.UserTypeID = new SelectList(db.UserTypeTables.Where(u => u.UserTypeID != 1 && u.UserTypeID != 3), "UserTypeID", "UserType", "0");
            return View("CreateUser");
        }

        private bool IsPasswordValid(string password)
        {
            if (password.Length < 8)
                return false;

            bool hasUppercase = password.Any(char.IsUpper);
            bool hasLowercase = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialCharacter = password.Any(ch => "!@#$%^&*()-_=+".Contains(ch));

            return hasUppercase && hasLowercase && hasDigit && hasSpecialCharacter;
        }

        public ActionResult AddDoctor()
        {
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Name", "0");
            ViewBag.AccountTypeID = new SelectList(db.AccountTypeTables.ToList(), "AccountTypeID", "Name", "0");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDoctor(DoctorTable doctor)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login");
            }

            var user = (UserTable)Session["User"];
            doctor.UserID = user.UserID;

            if (ModelState.IsValid)
            {
                var existingDoctor = db.DoctorTables.FirstOrDefault(d => d.EmailAddress == doctor.EmailAddress);
                if (existingDoctor != null)
                {
                    ViewBag.Message = "Email Already Registered!";
                }
                else
                {
                    // Check if the LogoFile property of the doctor object is not null
                    if (doctor.LogoFile != null)
                    {

                        // Generate a unique file name using a GUID
                        var folder = "~/Content/DoctorImages";
                        var uniqueFileName = Guid.NewGuid().ToString();
                        var file = string.Format("{0}.png", uniqueFileName);
                        Debug.WriteLine("Generated Unique File Name: " + file);
                        var response = FileHelpers.UploadPhoto(doctor.LogoFile, folder, file);
                        if (response)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            doctor.Photo = pic;
                            Debug.WriteLine("Image uploaded and path set: " + pic);
                        }
                    }


                    if (doctor.DocumentFile != null)
                    {
                        var docfolder = "~/Content/DoctorDocuments";
                        var uniqueDocFileName = Guid.NewGuid().ToString(); 
                        var docfile = string.Format("{0}.png", uniqueDocFileName);
                        var docresponse = FileHelpers.UploadDocument(doctor.DocumentFile, docfolder, docfile);
                        if (docresponse)
                        {
                            var pic = string.Format("{0}/{1}", docfolder, docfile);
                            doctor.SupportiveDocument = pic;
                        }
                    }

                    try
                    {
                        db.DoctorTables.Add(doctor);
                        db.SaveChanges();
                        return View("UnderReview");
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        // Log the error (uncomment the line below after adding a logging mechanism)
                        // Log.Error(ex, "Concurrency error occurred while adding doctor.");

                        ModelState.AddModelError("", "An error occurred while saving the doctor details. Please try again.");
                    }
                }
            }

            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Name", doctor.GenderID);
            ViewBag.AccountTypeID = new SelectList(db.AccountTypeTables.ToList(), "AccountTypeID", "Name", doctor.AccountTypeID);
            return View(doctor);
        }


        public ActionResult AddLab()
        {
            ViewBag.AccountTypeID = new SelectList(db.AccountTypeTables.ToList(), "AccountTypeID", "Name", "0");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLab(LabTable lab)
        {
            if (Session["User"] != null)
            {
                var user = (UserTable)Session["User"];
                lab.UserID = user.UserID;
                if (ModelState.IsValid)
                {
                    var findlab = db.LabTables.Where(d => d.EmailAddress == lab.EmailAddress).FirstOrDefault();
                    if (findlab == null)
                    {
                        db.LabTables.Add(lab);
                        db.SaveChanges();
                        if (lab.LogoFile != null)
                        {
                            var folder = "~/Content/LabImages";
                            var uniqueFileName = Guid.NewGuid().ToString(); 
                            var file = string.Format("{0}.png", uniqueFileName);
                            var response = FileHelpers.UploadPhoto(lab.LogoFile, folder, file);
                            if (response)
                            {
                                var pic = string.Format("{0}/{1}", folder, file);
                                lab.Photo = pic;
                                db.Entry(lab).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewBag.Message = "Email Already Registered!";
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }

            ViewBag.AccountTypeID = new SelectList(db.AccountTypeTables.ToList(), "AccountTypeID", "Name", lab.AccountTypeID);
            return View(lab);
        }

        public ActionResult AddPatient()
        {
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Name", "0");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPatient(PatientTable patient)
        {
            if (Session["User"] != null)
            {
                var user = (UserTable)Session["User"];
                patient.UserID = user.UserID;
                if (ModelState.IsValid)
                {
                    var findpatient = db.PatientTables.Where(d => d.Email == patient.Email).FirstOrDefault();
                    if (findpatient == null)
                    {
                        db.PatientTables.Add(patient);
                        db.SaveChanges();
                        if (patient.LogoFile != null)
                        {
                            var folder = "~/Content/PatientImages";
                            var uniqueFileName = Guid.NewGuid().ToString(); 
                            var file = string.Format("{0}.png", uniqueFileName);
                            var response = FileHelpers.UploadPhoto(patient.LogoFile, folder, file);
                            if (response)
                            {
                                var pic = string.Format("{0}/{1}", folder, file);
                                patient.Photo = pic;
                                db.Entry(patient).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewBag.Message = "Email Already Registered!";
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }

            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Name", patient.GenderID);
            return View(patient);
        }

        public ActionResult UnderReview()
        {
            return View();
        }

        public ActionResult UserLogOut()
        {
            LogOut();
            Session.Clear();
            return RedirectToAction("StartTemplate");
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